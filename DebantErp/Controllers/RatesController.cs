using DebantErp.BL.ProductionRate;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Расценки». Требует логина (deny-by-default fallback policy).
[Route("workspace/rates")]
public class RatesController : WorkspaceBaseController
{
    private readonly IProductionRate _rate;
    private readonly IProductionOperation _operation;

    public RatesController(IProductionRate rate, IProductionOperation operation)
    {
        _rate = rate;
        _operation = operation;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _rate.GetRates())
            .OrderBy(r => r.OperationName)
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var pageItems = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var rows = new List<ProductionRateRow>();
        foreach (var r in pageItems)
        {
            var history = (await _rate.GetHistory(r.ProductionOperationId ?? 0))
                .Where(h => !h.IsActual)   // только прошлые версии
                .ToList();
            rows.Add(new ProductionRateRow { Rate = r, History = history });
        }

        // Операции, у которых ещё нет действующей расценки, — для дропдауна добавления.
        var busy = all.Where(r => r.ProductionOperationId.HasValue)
            .Select(r => r.ProductionOperationId!.Value)
            .ToHashSet();
        var available = (await _operation.GetOperations())
            .Where(o => !busy.Contains(o.Id))
            .OrderBy(o => o.Name)
            .ToList();

        return View(new ProductionRateListViewModel
        {
            Items = rows,
            Page = page,
            TotalPages = totalPages,
            Edit = edit,
            AvailableOperations = available,
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductionRateDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте операцію, норму часу та ставку.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        await _rate.Create(dto);
        TempData["Success"] = "Розцінку додано.";
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/change")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Change(int id, ChangeProductionRateDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте норму часу та ставку.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _rate.Change(id, dto);
        TempData["Success"] = "Розцінку змінено — попередню версію збережено в історії.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _rate.Delete(id);
        TempData["Success"] = "Розцінку видалено.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }
}
