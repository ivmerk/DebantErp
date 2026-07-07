using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DebantErp.BL.Auth;
using DebantErp.ViewModels;

namespace DebantErp.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{

  private readonly IAuth authBL;

  public LoginController(IAuth authBL)
  {
    this.authBL = authBL;
  }
  [HttpGet]
  [Route("/login")]
  public IActionResult Index()
  {
    return View("Index", new LoginViewModel());
  }
  [HttpPost]
  [Route("/login")]
  public async Task<IActionResult> IndexSave(LoginViewModel model)
  {
    if (ModelState.IsValid)
    {
      try
      {
        await authBL.Authenticate(model.Email!, model.Password!, model.RememberMe == true);
        return Redirect("/workspace");
      }
      catch (DebantErp.BL.AuthorizationException)
      {
        ModelState.AddModelError("Email", "Ім'я або Email неправильні");
      }
    }
    return View("Index", model);

  }

  [HttpPost]
  [Route("/logout")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Logout()
  {
    await authBL.Logout();
    return Redirect("/");
  }
}

