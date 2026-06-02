
namespace DebantErp.Rdos
{
  public class OrderLaborCostRdo
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ProductionRateId { get; set; }
    public decimal Quantity { get; set; }
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
