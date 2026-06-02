using DebantErp.BL.Auth;
using Microsoft.AspNetCore.Mvc;
using DebantErp.ViewModels;
using DebantErp.ViewMapper;
namespace DebantErp.Controllers;
public class RegisterController : Controller
{
    private readonly IAuth authBL;
    public RegisterController(IAuth authBL)
    {
        this.authBL = authBL;
    }

    [HttpGet]
    [Route("/register")]
    public IActionResult Index()
    {
        return View("Index", new RegisterViewModel());
    }
    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> IndexSave(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var errorModel = await authBL.ValidateEmail(model.Email ?? "");
            if (errorModel != null)
            {
                ModelState.TryAddModelError("Email", errorModel.ErrorMessage!);
            }
        }
        if (ModelState.IsValid)
        {
            await authBL.CreateUser(AuthMapper.MapRegistrationViewModelToUserModel(model));
            return Redirect("/");
        }
        return View("Index", model);
    }
}
