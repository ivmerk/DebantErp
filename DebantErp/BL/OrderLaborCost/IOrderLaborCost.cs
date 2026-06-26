using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.OrderLaborCost
{
  public interface IOrderLaborCost
  {
    Task<List<OrderLaborCostRdo>> Get();
    Task<List<OrderLaborCostRdo>> GetByOrder(int orderId);
    Task<List<OrderLaborCostRdo>> GetByEmployee(int employeeId);
    Task<OrderLaborCostRdo> Get(int id);
    Task<int> Create(CreateOrderLaborCostDto dto);
    Task<int> Update(int id, UpdateOrderLaborCostDto dto);
    Task<int> Delete(int id);
  }
}
