using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderRdo Order { get; set; } = new();
        public List<LaborCostView> Costs { get; set; } = new();
        public decimal Total { get; set; }
        public bool Edit { get; set; }
        public List<EmployeeRdo> Employees { get; set; } = new();   // для дропдауна (действующие)
        public List<ProductionRateRdo> Rates { get; set; } = new(); // действующие расценки
    }

    public class LaborCostView
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = "";
        public string OperationName { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Sum { get; set; }
    }
}
