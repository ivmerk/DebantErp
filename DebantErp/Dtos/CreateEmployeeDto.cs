using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Отчество обязательно для заполнения")]
        public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен для заполнения")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "ИНН обязателен для заполнения")]
        public string TaxCode { get; set; } = null!;

        [Required(ErrorMessage = "Адрес обязателен для заполнения")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Почта обязательна для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Дата рождения обязательна для заполнения")]
        public string BirthDate { get; set; } = null!;

        [Required(ErrorMessage = "Пол обязателен для заполнения")]
        public string Gender { get; set; } = null!;
    }
}
