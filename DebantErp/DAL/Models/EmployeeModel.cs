namespace DebantErp.DAL.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public bool IsActual { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public EmployeeDetailsModel? EmployeeDetails { get; set; }
    }
}
