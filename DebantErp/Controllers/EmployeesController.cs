using DebantErp.BL.Employee;
using DebantErp.BL.Order;
using DebantErp.BL.OrderLaborCost;
using DebantErp.BL.ProductionRate;
using DebantErp.BL.Specialty;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Работники». Требует логина (deny-by-default fallback policy).
[Route("workspace/employees")]
public class EmployeesController : WorkspaceBaseController
{
    private readonly IEmployee _employee;
    private readonly IEmployeeDetails _employeeDetails;
    private readonly IEmployeeSpecialtyAssignment _assignment;
    private readonly ISpecialty _specialty;
    private readonly IOrderLaborCost _laborCost;
    private readonly IOrder _order;
    private readonly IProductionRate _rate;

    public EmployeesController(
        IEmployee employee,
        IEmployeeDetails employeeDetails,
        IEmployeeSpecialtyAssignment assignment,
        ISpecialty specialty,
        IOrderLaborCost laborCost,
        IOrder order,
        IProductionRate rate)
    {
        _employee = employee;
        _employeeDetails = employeeDetails;
        _assignment = assignment;
        _specialty = specialty;
        _laborCost = laborCost;
        _order = order;
        _rate = rate;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _employee.Get())                                     // мягко удалённых отсекает DAL
            .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)                // сортировка по фамилии
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var pageItems = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var specialties = await _specialty.GetSpecialties();
        var specialtyNames = specialties.ToDictionary(s => s.Id, s => s.Name);

        var rows = new List<EmployeeRow>();
        foreach (var e in pageItems)
        {
            var details = await _employeeDetails.GetEmployeeDetailsByEmployeeId(e.Id);
            var assignments = (await _assignment.GetByEmployee(e.Id))
                .Select(a => new EmployeeAssignmentView
                {
                    Id = a.Id,
                    SpecialtyName = specialtyNames.TryGetValue(a.SpecialtyId, out var name) ? name : $"#{a.SpecialtyId}",
                    DateFrom = a.DateFrom,
                })
                .OrderBy(a => a.SpecialtyName)
                .ToList();
            rows.Add(new EmployeeRow { Employee = e, Details = details, Assignments = assignments });
        }

        return View(new EmployeeListViewModel
        {
            Items = rows,
            Page = page,
            TotalPages = totalPages,
            Edit = edit,
            Specialties = specialties.OrderBy(s => s.Name).ToList(),
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте правильність заповнення полів.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        try
        {
            await _employee.Create(dto);
            TempData["Success"] = $"Працівника «{dto.LastName} {dto.FirstName}» додано.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Не вдалося додати працівника.";
        }
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте правильність заповнення полів.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _employee.Update(id, dto);
        TempData["Success"] = "Дані працівника оновлено.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _employee.Delete(id);
        TempData["Success"] = "Працівника видалено.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/specialties")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignSpecialty(int id, int specialtyId, string? dateFrom, int page = 1)
    {
        if (specialtyId <= 0 || string.IsNullOrWhiteSpace(dateFrom)
            || !DateTime.TryParse(dateFrom, out var newDate))
        {
            TempData["Error"] = "Оберіть спеціальність і дату початку.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }

        // Новая специальность не может начинаться раньше последней уже назначенной
        // (даты назначений у работника не должны идти назад).
        var current = await _assignment.GetByEmployee(id);
        if (current.Count > 0)
        {
            var latest = current.Max(a => a.DateFrom);
            if (newDate.Date < latest.Date)
            {
                TempData["Error"] = $"Дата початку не може бути раніше за останню призначену спеціальність ({latest:yyyy-MM-dd}).";
                return RedirectToAction(nameof(Index), new { page, edit = true });
            }
        }

        try
        {
            await _assignment.Create(new CreateEmployeeAssignmentDto
            {
                EmployeeId = id,
                SpecialtyId = specialtyId,
                DateFrom = dateFrom,
            });
            TempData["Success"] = "Спеціальність призначено.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Не вдалося призначити спеціальність.";
        }
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/specialties/{assignmentId:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveSpecialty(int id, int assignmentId, int page = 1)
    {
        await _assignment.Delete(assignmentId);
        TempData["Success"] = "Спеціальність знято.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    // Трудозатраты работника за период.
    [HttpGet("{id:int}/labor-costs")]
    public async Task<IActionResult> LaborCosts(int id, DateTime? from, DateTime? to)
    {
        var employee = await _employee.Get(id);
        if (employee.Id == 0)
        {
            TempData["Error"] = "Працівника не знайдено.";
            return RedirectToAction(nameof(Index));
        }

        // По умолчанию — текущий месяц.
        var today = DateTime.Today;
        var fromDate = (from ?? new DateTime(today.Year, today.Month, 1)).Date;
        var toDate = (to ?? today).Date;

        // Период фильтруется по дате заказа (отдельной даты у трудозатраты нет).
        var orders = (await _order.Get()).ToDictionary(o => o.Id, o => o);

        var costs = (await _laborCost.GetByEmployee(id))
            .Where(c => orders.TryGetValue(c.OrderId, out var o)
                        && o.CreatedAt.Date >= fromDate && o.CreatedAt.Date <= toDate)
            .ToList();

        var rateCache = new Dictionary<int, Rdos.ProductionRateRdo>();
        foreach (var c in costs)
        {
            if (!rateCache.ContainsKey(c.ProductionRateId))
                rateCache[c.ProductionRateId] = await _rate.GetRate(c.ProductionRateId);
        }

        var rows = costs
            .Select(c =>
            {
                var order = orders[c.OrderId];
                var rate = rateCache[c.ProductionRateId];
                return new EmployeeLaborCostRow
                {
                    Date = order.CreatedAt,
                    OrderNumber = order.Number ?? $"#{c.OrderId}",
                    OperationName = rate.OperationName,
                    Rate = rate.Rate,
                    Quantity = c.Quantity,
                    Sum = rate.Rate * c.Quantity,
                };
            })
            .OrderByDescending(r => r.Date)
            .ToList();

        return View(new EmployeeLaborCostsViewModel
        {
            Employee = employee,
            From = fromDate,
            To = toDate,
            Rows = rows,
            Total = rows.Sum(r => r.Sum),
        });
    }
}
