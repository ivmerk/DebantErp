using DebantErp.BL.Specialty;
using DebantErp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Хаб выбора сущности + разделы. Требует логина (deny-by-default fallback policy).
[Route("workspace")]
public class WorkspaceController : Controller
{
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
    public async Task<IActionResult> Specialties()
    {
        var all = await _specialty.GetSpecialties();
        // мягко удалённые (is_actual = false) в списке не показываем
        return View(all.Where(s => s.IsActual).ToList());
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
    public async Task<IActionResult> SpecialtyEdit(int id, CreateUpdateSpecialtyDto dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Название не может быть пустым.";
            return RedirectToAction(nameof(Specialties));
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
        return RedirectToAction(nameof(Specialties));
    }

    [HttpPost("specialties/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SpecialtyDelete(int id)
    {
        await _specialty.Delete(id);
        TempData["Success"] = "Специальность удалена.";
        return RedirectToAction(nameof(Specialties));
    }
}
