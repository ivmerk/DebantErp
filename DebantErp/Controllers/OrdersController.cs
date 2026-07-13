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
            TempData["Error"] = "Вкажіть номер замовлення.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        try
        {
            await _order.Create(dto);
            TempData["Success"] = $"Замовлення «{dto.Number}» створено.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Замовлення «{dto.Number}» вже існує.";
        }
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateOrderDto dto, int page = 1)
    {
        if ((await _order.Get(id)).Id == 0)
        {
            TempData["Error"] = "Замовлення не знайдено.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        try
        {
            await _order.Update(id, dto);
            TempData["Success"] = "Замовлення оновлено.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Замовлення «{dto.Number}» вже існує.";
        }
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        if ((await _order.Get(id)).Id == 0)
        {
            TempData["Error"] = "Замовлення не знайдено.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _order.Delete(id);
        TempData["Success"] = "Замовлення видалено.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    // ---------- Трудозатраты по заказу ----------

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, bool edit = false)
    {
        var order = await _order.Get(id);
        if (order.Id == 0)
        {
            TempData["Error"] = "Замовлення не знайдено.";
            return RedirectToAction(nameof(Index));
        }

        var employees = await _employee.Get();
        var empNames = employees.ToDictionary(e => e.Id, e => $"{e.LastName} {e.FirstName}");

        var costs = await _laborCost.GetByOrder(id);

        // Расценку каждой версии резолвим один раз (несколько строк могут ссылаться
        // на одну расценку) — иначе GetRate дёргается на каждую трудозатрату.
        var rateCache = new Dictionary<int, Rdos.ProductionRateRdo>();
        foreach (var c in costs)
        {
            if (!rateCache.ContainsKey(c.ProductionRateId))
                rateCache[c.ProductionRateId] = await _rate.GetRate(c.ProductionRateId);
        }

        var views = costs.Select(c =>
        {
            var rate = rateCache[c.ProductionRateId];
            return new LaborCostView
            {
                Id = c.Id,
                EmployeeName = empNames.TryGetValue(c.EmployeeId, out var n) ? n : $"#{c.EmployeeId}",
                OperationName = rate.OperationName,
                Rate = rate.CostPerPiece,
                Quantity = c.Quantity,
                Sum = rate.CostPerPiece * c.Quantity,
            };
        }).ToList();

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
            TempData["Error"] = "Оберіть працівника, розцінку та вкажіть кількість.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        await _laborCost.Create(new CreateOrderLaborCostDto
        {
            OrderId = id,
            EmployeeId = employeeId,
            ProductionRateId = productionRateId,
            Quantity = quantity,
        });
        TempData["Success"] = "Трудовитрату додано.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }

    [HttpPost("{id:int}/costs/{costId:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditLaborCost(int id, int costId, int quantity)
    {
        if (quantity <= 0)
        {
            TempData["Error"] = "Кількість має бути більшою за нуль.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        if ((await _laborCost.Get(costId)).OrderId != id)
        {
            TempData["Error"] = "Трудовитрата не належить цьому замовленню.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        await _laborCost.Update(costId, new UpdateOrderLaborCostDto { Quantity = quantity });
        TempData["Success"] = "Кількість оновлено.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }

    [HttpPost("{id:int}/costs/{costId:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteLaborCost(int id, int costId)
    {
        if ((await _laborCost.Get(costId)).OrderId != id)
        {
            TempData["Error"] = "Трудовитрата не належить цьому замовленню.";
            return RedirectToAction(nameof(Details), new { id, edit = true });
        }
        await _laborCost.Delete(costId);
        TempData["Success"] = "Трудовитрату видалено.";
        return RedirectToAction(nameof(Details), new { id, edit = true });
    }
}
