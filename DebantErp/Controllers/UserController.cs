using DebantErp.BL.Auth;
using DebantErp.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IAuth _auth;

    public UserController(IAuth auth)
    {
        _auth = auth;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _auth.GetUser(id);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Данные отсутствуют.");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = await _auth.UpdateUser(id, dto);
        return Ok(userId);
    }
}
