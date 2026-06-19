using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Хаб выбора сущности. Требует логина (deny-by-default fallback policy).
// Разделы пока заглушки — наполняются по очереди.
[Route("workspace")]
public class WorkspaceController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("employees")]
    public IActionResult Employees() => View("Section", "Работники");

    [HttpGet("specialties")]
    public IActionResult Specialties() => View("Section", "Специальности");

    [HttpGet("rates")]
    public IActionResult Rates() => View("Section", "Расценки");

    [HttpGet("orders")]
    public IActionResult Orders() => View("Section", "Заказы");
}
