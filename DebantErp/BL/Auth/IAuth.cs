using DebantErp.Dtos;
using DebantErp.Rdos;
using System.ComponentModel.DataAnnotations;
using DebantErp.DAL.Models;

namespace DebantErp.BL.Auth;

public interface IAuth
{
  Task<int> CreateUser(UserModel model);
  Task<int> Authenticate(string email, string password, bool rememberMe);
  Task Logout();
  Task<UserRdo> GetUser(int id);
  Task<int> UpdateUser(int id, UpdateUserDto dto);
  Task<ValidationResult?> ValidateEmail(string email);

}