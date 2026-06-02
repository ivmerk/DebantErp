using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos;
public class RegisterUserDto
{
    [Required(ErrorMessage = "Имя обязательно")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Имя обязательно")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Телефон обязателен")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    public string? Password { get; set; }
}
