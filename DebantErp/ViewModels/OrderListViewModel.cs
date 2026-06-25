using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class OrderListViewModel
    {
        public List<OrderRdo> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public bool Edit { get; set; }
    }
}
