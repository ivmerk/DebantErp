using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Хаб выбора сущности. Требует логина (deny-by-default fallback policy).
// Разделы вынесены в отдельные контроллеры: EmployeesController, SpecialtiesController.
[Route("workspace")]
public class WorkspaceController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    // --- заглушки (наполняются по очереди, потом вынесем в свои контроллеры) ---
    [HttpGet("rates")]
    public IActionResult Rates() => View("Section", "Расценки");

    [HttpGet("orders")]
    public IActionResult Orders() => View("Section", "Заказы");
}
