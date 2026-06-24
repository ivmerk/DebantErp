using DebantErp.BL.Employee;
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

    public EmployeesController(
        IEmployee employee,
        IEmployeeDetails employeeDetails,
        IEmployeeSpecialtyAssignment assignment,
        ISpecialty specialty)
    {
        _employee = employee;
        _employeeDetails = employeeDetails;
        _assignment = assignment;
        _specialty = specialty;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _employee.Get())
            .Where(e => bool.TryParse(e.IsActual, out var actual) && actual) // мягко удалённых не показываем
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
            Specialties = specialties.Where(s => s.IsActual).OrderBy(s => s.Name).ToList(),
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Проверьте корректность заполнения полей.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        try
        {
            await _employee.Create(dto);
            TempData["Success"] = $"Работник «{dto.LastName} {dto.FirstName}» добавлен.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Не удалось добавить работника.";
        }
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Проверьте корректность заполнения полей.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _employee.Update(id, dto);
        TempData["Success"] = "Данные работника обновлены.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _employee.Delete(id);
        TempData["Success"] = "Работник удалён.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/specialties")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignSpecialty(int id, int specialtyId, string? dateFrom, int page = 1)
    {
        if (specialtyId <= 0 || string.IsNullOrWhiteSpace(dateFrom)
            || !DateTime.TryParse(dateFrom, out var newDate))
        {
            TempData["Error"] = "Выберите специальность и дату начала.";
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
                TempData["Error"] = $"Дата начала не может быть раньше последней назначенной специальности ({latest:yyyy-MM-dd}).";
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
            TempData["Success"] = "Специальность назначена.";
        }
        catch (Exception)
        {
            TempData["Error"] = "Не удалось назначить специальность.";
        }
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/specialties/{assignmentId:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveSpecialty(int id, int assignmentId, int page = 1)
    {
        await _assignment.Delete(assignmentId);
        TempData["Success"] = "Специальность снята.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }
}
