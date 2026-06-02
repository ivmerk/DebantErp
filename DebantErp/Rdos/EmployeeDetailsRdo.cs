namespace DebantErp.Rdos
{
    public class EmployeeDetailsRdo
    {
        public int Id { get; set; }
        public string TaxCode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string BirthDate { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public int EmployeeId { get; set; }
    }
}
