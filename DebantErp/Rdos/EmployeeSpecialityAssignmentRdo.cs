namespace DebantErp.Rdos
{
    public class EmployeeSpecialityAssignmentRdo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int SpecialityId { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
