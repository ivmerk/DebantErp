using DebantErp.BL.Specialty;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Специальности». Требует логина (deny-by-default fallback policy).
[Route("workspace/specialties")]
public class SpecialtiesController : WorkspaceBaseController
{
    private readonly ISpecialty _specialty;

    public SpecialtiesController(ISpecialty specialty)
    {
        _specialty = specialty;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _specialty.GetSpecialties())
            .Where(s => s.IsActual)            // мягко удалённые не показываем
            .OrderBy(s => s.Name)              // сортировка по названию
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        return View(new SpecialtyListViewModel { Items = items, Page = page, TotalPages = totalPages, Edit = edit });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUpdateSpecialtyDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        try
        {
            await _specialty.Create(dto);
            TempData["Success"] = $"Специальность «{dto.Name}» добавлена.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Специальность «{dto.Name}» уже существует.";
        }
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateUpdateSpecialtyDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        try
        {
            await _specialty.Update(id, dto);
            TempData["Success"] = "Специальность обновлена.";
        }
        catch (Exception)
        {
            TempData["Error"] = $"Специальность «{dto.Name}» уже существует.";
        }
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _specialty.Delete(id);
        TempData["Success"] = "Специальность удалена.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }
}
