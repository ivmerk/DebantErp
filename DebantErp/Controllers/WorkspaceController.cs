using DebantErp.BL.Specialty;
using DebantErp.Dtos;
using DebantErp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Хаб выбора сущности + разделы. Требует логина (deny-by-default fallback policy).
[Route("workspace")]
public class WorkspaceController : Controller
{
    private const int PageSize = 20;

    private readonly ISpecialty _specialty;

    public WorkspaceController(ISpecialty specialty)
    {
        _specialty = specialty;
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    // --- заглушки (наполняются по очереди) ---
    [HttpGet("employees")]
    public IActionResult Employees() => View("Section", "Работники");

    [HttpGet("rates")]
    public IActionResult Rates() => View("Section", "Расценки");

    [HttpGet("orders")]
    public IActionResult Orders() => View("Section", "Заказы");

    // --- Специальности ---
    [HttpGet("specialties")]
    public async Task<IActionResult> Specialties(int page = 1)
    {
        var all = (await _specialty.GetSpecialties())
            .Where(s => s.IsActual)            // мягко удалённые не показываем
            .OrderBy(s => s.Name)              // сортировка по названию
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(all.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        var items = all.Skip((page - 1) * PageSize).Take(PageSize).ToList();
        return View(new SpecialtyListViewModel { Items = items, Page = page, TotalPages = totalPages });
    }

    [HttpPost("specialties/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SpecialtyCreate(CreateUpdateSpecialtyDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Specialties));
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
        return RedirectToAction(nameof(Specialties));
    }

    [HttpPost("specialties/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SpecialtyEdit(int id, CreateUpdateSpecialtyDto dto, int page = 1)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Specialties), new { page });
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
        return RedirectToAction(nameof(Specialties), new { page });
    }

    [HttpPost("specialties/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SpecialtyDelete(int id, int page = 1)
    {
        await _specialty.Delete(id);
        TempData["Success"] = "Специальность удалена.";
        return RedirectToAction(nameof(Specialties), new { page });
    }
}
