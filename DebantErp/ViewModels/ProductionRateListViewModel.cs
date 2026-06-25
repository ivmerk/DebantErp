using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class ProductionRateListViewModel
    {
        public List<ProductionRateRow> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public bool Edit { get; set; }
        // Операции без действующей расценки — для формы добавления.
        public List<ProductionOperationRdo> AvailableOperations { get; set; } = new();
    }

    public class ProductionRateRow
    {
        public ProductionRateRdo Rate { get; set; } = new();
        public List<ProductionRateRdo> History { get; set; } = new(); // прошлые версии
    }
}
