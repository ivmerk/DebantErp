using DebantErp.BL.Auth;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DebantErp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IAuth _auth;

    public AdminController(IAuth auth)
    {
        _auth = auth;
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        var users = await _auth.GetUsers();
        return View(users);
    }

    [HttpPost("users/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(int id, UserRoleEnum role, UserStatusEnum status)
    {
        await _auth.UpdateUser(id, new UpdateUserDto { Role = role, Status = status });
        return RedirectToAction(nameof(Users));
    }
}
