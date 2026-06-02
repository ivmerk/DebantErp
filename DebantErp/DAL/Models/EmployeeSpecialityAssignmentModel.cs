namespace DebantErp.DAL.Models
{
    public class EmployeeSpecialityAssignmentModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int SpecialityId { get; set; }

        public bool IsActual { get; set; } = true;

        public DateTime DateFrom { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
