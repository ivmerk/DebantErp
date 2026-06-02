namespace DebantErp.DAL.Models
{
    public class EmployeeDetailsModel
    {
        public int Id { get; set; }

        public string TaxCode { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public GenderEnum Gender { get; set; }

        public string Picture { get; set; } = null!;

        public int EmployeeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
