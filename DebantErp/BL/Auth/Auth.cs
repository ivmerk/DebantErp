using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DebantErp.BL.Auth;

public class Auth : IAuth
{
  private readonly IAuthDAL _authDAL;
  private readonly IEncrypt _encrypt;
  private readonly IHttpContextAccessor _httpContextAccessor;


  public Auth(IAuthDAL authDAL, IEncrypt encrypt, IHttpContextAccessor httpContextAccessor)
  {
    _authDAL = authDAL;
    _encrypt = encrypt;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<int> CreateUser(UserModel user)
  {
    if (user == null)
      throw new ArgumentNullException(nameof(user));

    user.Salt = Guid.NewGuid().ToString();
    user.Status = UserStatusEnum.NeedToApprove;
    user.CreatedAt = DateTime.UtcNow;
    user.UpdatedAt = DateTime.UtcNow;
    user.Password = _encrypt.HashPassword(user.Password ?? "", user.Salt);

    return await _authDAL.Create(user);
  }

  public async Task<int> Authenticate(string email, string password, bool rememberMe)
  {
    var user = await _authDAL.Get(email);

    if (user.Id != null && user.Password == _encrypt.HashPassword(password, user.Salt))
    {
      await SignInAsync(user, rememberMe);
      return user.Id.Value;
    }

    throw new AuthorizationException();
  }

  public async Task Logout()
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext != null)
      await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
  }

  private async Task SignInAsync(UserModel user, bool isPersistent)
  {
    var httpContext = _httpContextAccessor.HttpContext
      ?? throw new InvalidOperationException("No HttpContext available for sign-in.");

    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id!.Value.ToString()),
      new(ClaimTypes.Name, user.Email ?? ""),
      new(ClaimTypes.Role, user.Role.ToString()),
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    var props = new AuthenticationProperties
    {
      IsPersistent = isPersistent,
      ExpiresUtc = isPersistent
        ? DateTimeOffset.UtcNow.AddDays(30)
        : DateTimeOffset.UtcNow.AddMinutes(30),
    };

    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
  }

  public async Task<UserRdo> GetUser(int id)
  {
    var user = await _authDAL.Get(id);
    return new UserRdo
    {
      Id = user.Id ?? -1,
      FirstName = user.FirstName ?? "",
      LastName = user.LastName ?? "",
      Phone = user.Phone ?? "",
      Role = user.Role,
      Email = user.Email ?? "",
      Status = user.Status,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt,
    };
  }

  public async Task<List<UserRdo>> GetUsers()
  {
    var users = await _authDAL.Get();
    return users.Select(user => new UserRdo
    {
      Id = user.Id ?? -1,
      FirstName = user.FirstName ?? "",
      LastName = user.LastName ?? "",
      Phone = user.Phone ?? "",
      Role = user.Role,
      Email = user.Email ?? "",
      Status = user.Status,
      CreatedAt = user.CreatedAt,
      UpdatedAt = user.UpdatedAt,
    }).ToList();
  }

  public async Task<int> UpdateUser(int id, UpdateUserDto model)
  {
    var user = await GetUser(id);
    var updatedUser = new UserModel
    {
      Id = id,
      FirstName = !string.IsNullOrEmpty(model.FirstName) ? model.FirstName : user.FirstName,
      LastName = !string.IsNullOrEmpty(model.LastName) ? model.LastName : user.LastName,
      Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : user.Phone,
      Email = !string.IsNullOrEmpty(model.Email) ? model.Email : user.Email,
      Role = model.Role != null && Enum.IsDefined(typeof(UserRoleEnum), model.Role.Value) ? model.Role.Value : user.Role,
      Status = model.Status != null && Enum.IsDefined(typeof(UserStatusEnum), model.Status.Value) ? model.Status.Value : user.Status,
    };
    return await _authDAL.Update(updatedUser);
  }

  public async Task<ValidationResult?> ValidateEmail(string email)
  {
    var user = await _authDAL.Get(email);
    if (user.Id != null)
      return new ValidationResult("Користувач з таким email вже існує.");
    return null;
  }
}
