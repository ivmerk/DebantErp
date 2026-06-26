using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
  public interface IOrderLaborCostDAL : IBaseDAL<OrderLaborCostModel>
  {
    Task<List<OrderLaborCostModel>> GetByOrderId(int orderId);
    Task<List<OrderLaborCostModel>> GetByEmployeeId(int employeeId);
  }
}
