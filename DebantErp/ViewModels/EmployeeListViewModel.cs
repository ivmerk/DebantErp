using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class EmployeeListViewModel
    {
        public List<EmployeeRow> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public bool Edit { get; set; }   // режим редактирования (по умолчанию — только просмотр)
        public List<SpecialtyRdo> Specialties { get; set; } = new(); // для выпадающего списка назначения
    }

    public class EmployeeRow
    {
        public EmployeeRdo Employee { get; set; } = new();
        public EmployeeDetailsRdo Details { get; set; } = new();
        public List<EmployeeAssignmentView> Assignments { get; set; } = new();
    }

    public class EmployeeAssignmentView
    {
        public int Id { get; set; }                 // id назначения (для снятия)
        public int SpecialtyId { get; set; }        // для сопоставления с выпадающим списком
        public string SpecialtyName { get; set; } = "";
        public DateTime DateFrom { get; set; }
    }
}
