using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateEmployeeAssignmentDto
    {
        [Required(ErrorMessage = "EmployeeId is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "EmployeeId must be a number")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "SpecialityId is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "SpecialityId must be a number")]
        public int SpecialityId { get; set; }

        [Required(ErrorMessage = "DateFrom is required")]
        public string DateFrom { get; set; } = null!;
    }
}
