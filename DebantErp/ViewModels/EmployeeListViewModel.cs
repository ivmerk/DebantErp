using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class EmployeeListViewModel
    {
        public List<EmployeeRow> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }

    public class EmployeeRow
    {
        public EmployeeRdo Employee { get; set; } = new();
        public EmployeeDetailsRdo Details { get; set; } = new();
    }
}
