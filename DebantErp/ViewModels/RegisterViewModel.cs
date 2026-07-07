using System.ComponentModel.DataAnnotations;

namespace DebantErp.ViewModels;
public class RegisterViewModel
{
    [Required(ErrorMessage = "Email обов'язковий")]
    [EmailAddress(ErrorMessage = "Некоректний формат")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Пароль обов'язковий")]
 //   [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-]).{10,}$", ErrorMessage = "Пароль слишком простой")]
    public string? Password { get; set; }
}

