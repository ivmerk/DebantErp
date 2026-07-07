using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos;
public class RegisterUserDto
{
    [Required(ErrorMessage = "Ім'я обов'язкове")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Прізвище обов'язкове")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Телефон обов'язковий")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Некоректний Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Пароль обов'язковий")]
    public string? Password { get; set; }
}
