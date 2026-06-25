using DebantErp.BL.ProductionRate;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Производственные операции». Требует логина (deny-by-default fallback policy).
[Route("workspace/operations")]
public class ProductionOperationsController : WorkspaceBaseController
{
    private readonly IProductionOperation _operation;

    public ProductionOperationsController(IProductionOperation operation)
    {
        _operation = operation;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _operation.GetOperations())
            .OrderBy(o => o.Name)
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        return View(new ProductionOperationListViewModel { Items = items, Page = page, TotalPages = totalPages, Edit = edit });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUpdateProductionOperationDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        await _operation.Create(dto);
        TempData["Success"] = $"Операция «{dto.Name}» добавлена.";
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateUpdateProductionOperationDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _operation.Update(id, dto);
        TempData["Success"] = "Операция обновлена.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _operation.Delete(id);
        TempData["Success"] = "Операция удалена.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }
}
