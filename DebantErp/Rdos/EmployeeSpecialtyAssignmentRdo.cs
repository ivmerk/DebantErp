namespace DebantErp.Rdos
{
    public class EmployeeSpecialtyAssignmentRdo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SpecialtyId { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
