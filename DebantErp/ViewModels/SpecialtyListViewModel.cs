using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class SpecialtyListViewModel
    {
        public List<SpecialtyRdo> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
    }
}
