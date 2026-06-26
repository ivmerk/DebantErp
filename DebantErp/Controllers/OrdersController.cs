using DebantErp.BL.Employee;
using DebantErp.BL.Order;
using DebantErp.BL.OrderLaborCost;
using DebantErp.BL.ProductionRate;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Заказы» (заказ + трудозатраты). Требует логина.
[Route("workspace/orders")]
public class OrdersController : WorkspaceBaseController
{
    private readonly IOrder _order;
    private readonly IOrderLaborCost _laborCost;
    private readonly IEmployee _employee;
    private readonly IProductionRate _rate;

    public OrdersController(IOrder order, IOrderLaborCost laborCost, IEmployee employee, IProductionRate rate)
    {
        _order = order;
        _laborCost = laborCost;
        _employee = employee;
        _rate = rate;
    }

    // ---------- Заказы ----------

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _order.Get())
            .OrderByDescending(o => o.CreatedAt)
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        return View(new OrderListViewModel { Items = items, Page = page, TotalPages = totalPages, Edit = edit });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Number))
        {
            TempData["Error"] = "Укажите номер заказа.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        try
        {
            await _order.Create(dto);
            TempData["Success"] = $"Заказ «{dto.Number}» создан.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Заказ «{dto.Number}» уже существует.";
        }
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateOrderDto dto, int page = 1)
    {
        try
        {
            await _order.Update(id, dto);
            TempData["Success"] = "Заказ обновлён.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Заказ «{dto.Number}» уже существует.";
        }
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _order.Delete(id);
        TempData["Success"] = "Заказ удалён.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    // ---------- Трудозатраты по заказу ----------

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, bool edit = false)
    {
        var order = await _order.Get(id);
        if (order.Id == 0)
        {
            TempData["Error"] = "Заказ не найден.";
            return RedirectToAction(nameof(Index));
        }

        var employees = await _employee.Get();
        var empNames = employees.ToDictionary(e => e.Id, e => $"{e.LastName} {e.FirstName}");

        var costs = await _laborCost.GetByOrder(id);
        var views = new List<LaborCostView>();
        foreach (var c in costs)
        {
            var rate = await _rate.GetRate(c.ProductionRateId);
            views.Add(new LaborCostView
            {
                Id = c.Id,
                EmployeeName = empNames.TryGetValue(c.EmployeeId, out var n) ? n : $"#{c.EmployeeId}",
                OperationName = rate.OperationName,
                Rate = rate.Rate,
                Quantity = c.Quantity,
                Sum = rate.Rate * c.Quantity,
            });
        }

        return View(new OrderDetailsViewModel
        {
            Order = order,
            Costs = views,
            Total = views.Sum(v => v.Sum),
            Edit = edit,
            Employees = employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList(),
            Rates = (await _rate.GetRates()).OrderBy(r => r.OperationName).ToList(),
        });
    }

    [HttpPost("{id:int}/costs")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLaborCost(int id, int employeeId, int productionRateId, int quantity)
    {
        if (employeeId <= 0 || productionRateId <= 0 || quantity <= 0)
        {
            TempData["Error"] = "Выберите работника, расценку и укажите количество.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        await _laborCost.Create(new CreateOrderLaborCostDto
        {
            OrderId = id,
            EmployeeId = employeeId,
            ProductionRateId = productionRateId,
            Quantity = quantity,
        });
        TempData["Success"] = "Трудозатрата добавлена.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }

    [HttpPost("{id:int}/costs/{costId:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLaborCost(int id, int costId, int quantity)
    {
        if (quantity <= 0)
        {
            TempData["Error"] = "Количество должно быть больше нуля.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        await _laborCost.Update(costId, new UpdateOrderLaborCostDto { Quantity = quantity });
        TempData["Success"] = "Количество обновлено.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }

    [HttpPost("{id:int}/costs/{costId:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLaborCost(int id, int costId)
    {
        await _laborCost.Delete(costId);
        TempData["Success"] = "Трудозатрата удалена.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }
}
