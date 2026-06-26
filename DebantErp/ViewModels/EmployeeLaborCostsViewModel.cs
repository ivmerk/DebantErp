using DebantErp.Rdos;

namespace DebantErp.ViewModels
{
    public class EmployeeLaborCostsViewModel
    {
        public EmployeeRdo Employee { get; set; } = new();
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<EmployeeLaborCostRow> Rows { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class EmployeeLaborCostRow
    {
        public DateTime Date { get; set; }
        public string OrderNumber { get; set; } = "";
        public string OperationName { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Sum { get; set; }
    }
}
