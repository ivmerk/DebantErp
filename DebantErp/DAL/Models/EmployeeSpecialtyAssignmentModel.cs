namespace DebantErp.DAL.Models
{
    public class EmployeeSpecialtyAssignmentModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int SpecialtyId { get; set; }

        public bool IsActual { get; set; } = true;

        public DateTime DateFrom { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
