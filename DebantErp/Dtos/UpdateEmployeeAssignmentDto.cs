using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class UpdateEmployeeAssignmentDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be greater than 0")]
        public int? EmployeeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SpecialityId must be greater than 0")]
        public int? SpecialityId { get; set; }

        [DataType(DataType.Date)]
        public string? DateFrom { get; set; }
    }
}
