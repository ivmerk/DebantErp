using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DebantErp.Models;
using DebantErp.BL.Auth;

namespace DebantErp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrentUser _currentUser;

    public HomeController(ILogger<HomeController> logger, ICurrentUser currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;

    }

    public IActionResult Index()
    {
        return View(_currentUser.IsLoggedIn());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

