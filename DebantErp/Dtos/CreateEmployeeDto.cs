using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Ім'я обов'язкове для заповнення")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "По батькові обов'язкове для заповнення")]
        public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = "Прізвище обов'язкове для заповнення")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефону обов'язковий для заповнення")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "ІПН обов'язковий для заповнення")]
        public string TaxCode { get; set; } = null!;

        [Required(ErrorMessage = "Адреса обов'язкова для заповнення")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Пошта обов'язкова для заповнення")]
        [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Дата народження обов'язкова для заповнення")]
        public string BirthDate { get; set; } = null!;

        [Required(ErrorMessage = "Стать обов'язкова для заповнення")]
        public string Gender { get; set; } = null!;
    }
}
