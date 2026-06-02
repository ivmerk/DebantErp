namespace DebantErp.Rdos
{
    public class EmployeeRdo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string IsActual { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
