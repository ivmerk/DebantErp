using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? TaxCode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        [DataType(DataType.Date)]
        public string? BirthDate { get; set; }

        [RegularExpression("^(male|female)$", ErrorMessage = "Gender must be 'male' or 'female'")]
        public string? Gender { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }
    }
}
