using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class GradeListViewModel
    {
        public List<GradeRow> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public bool Edit { get; set; }
    }

    public class GradeRow
    {
        public GradeRdo Grade { get; set; } = new();
        public List<GradeRdo> History { get; set; } = new(); // прошлые версии
    }
}
