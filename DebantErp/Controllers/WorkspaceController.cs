using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

// Хаб выбора сущности. Требует логина (deny-by-default fallback policy).
// Разделы вынесены в отдельные контроллеры: Employees, Specialties,
// ProductionOperations, Rates, Orders.
[Route("workspace")]
public class WorkspaceController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}
