using DebantErp.BL.Grade;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Раздел «Разряды». Требует логина (deny-by-default fallback policy).
[Route("workspace/grades")]
public class GradesController : WorkspaceBaseController
{
    private readonly IGrade _grade;

    public GradesController(IGrade grade)
    {
        _grade = grade;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, bool edit = false)
    {
        var all = (await _grade.GetGrades())
            .OrderBy(g => g.Grade)
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var pageItems = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        var rows = new List<GradeRow>();
        foreach (var g in pageItems)
        {
            var history = (await _grade.GetHistory(g.Grade))
                .Where(h => !h.IsActual)   // только прошлые версии
                .ToList();
            rows.Add(new GradeRow { Grade = g, History = history });
        }

        return View(new GradeListViewModel
        {
            Items = rows,
            Page = page,
            TotalPages = totalPages,
            Edit = edit,
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGradeDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте розряд, денну ставку та дату введення.";
            return RedirectToAction(nameof(Index), new { edit = true });
        }
        await _grade.Create(dto);
        TempData["Success"] = "Розряд додано.";
        return RedirectToAction(nameof(Index), new { edit = true });
    }

    [HttpPost("{id:int}/change")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Change(int id, ChangeGradeDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Перевірте денну ставку та дату введення.";
            return RedirectToAction(nameof(Index), new { page, edit = true });
        }
        await _grade.Change(id, dto);
        TempData["Success"] = "Розряд змінено — попередню версію збережено в історії.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int page = 1)
    {
        await _grade.Delete(id);
        TempData["Success"] = "Розряд видалено.";
        return RedirectToAction(nameof(Index), new { page, edit = true });
    }
}
