using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateEmployeeAssignmentDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "EmployeeId must be a number")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "SpecialtyId is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "SpecialtyId must be a number")]
        public int SpecialtyId { get; set; }

        [Required(ErrorMessage = "DateFrom is required")]
        public string DateFrom { get; set; } = null!;
    }
}
