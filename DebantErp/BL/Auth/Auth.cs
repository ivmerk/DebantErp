using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;
using System.ComponentModel.DataAnnotations;

namespace DebantErp.BL.Auth;

public class Auth : IAuth
{
  private readonly IAuthDAL _authDAL;
  private readonly IEncrypt _encrypt;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IAuthSessionDAL _authSessionDAL;


  public Auth(IAuthDAL authDAL, IEncrypt encrypt, IHttpContextAccessor httpContextAccessor, IAuthSessionDAL authSessionDAL)
  {
    _authDAL = authDAL;
    _encrypt = encrypt;
    _httpContextAccessor = httpContextAccessor;
    _authSessionDAL = authSessionDAL;
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

  public void Login(int id)
  {
    _httpContextAccessor.HttpContext?.Session.SetInt32("userid", id);
  }

  public async Task<int> Authenticate(string email, string password, bool rememberMe)
  {
    var user = await _authDAL.Get(email);

    if (user.Id != null && user.Password == _encrypt.HashPassword(password, user.Salt))
    {
      var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
      var userAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();

      var session = new AuthSessionModel
      {
        SessionId = Guid.NewGuid(),
        UserId = user.Id.Value,
        SessionToken = Guid.NewGuid().ToString(),
        IpAddress = ipAddress,
        UserAgent = userAgent,
        CreatedAt = DateTime.UtcNow,
        LastAccessedAt = DateTime.UtcNow,
        ExpiresAt = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(30),
        IsActive = true,
      };

      await _authSessionDAL.Create(session);
      Login(user.Id.Value);
      return user.Id.Value;
    }

    throw new AuthorizationException();
  }

  public async Task Logout(int userId)
  {
    await _authSessionDAL.DeactivateByUserId(userId);
    _httpContextAccessor.HttpContext?.Session.Remove("userid");
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
      return new ValidationResult("Пользователь с таким email уже существует.");
    return null;
  }
}
