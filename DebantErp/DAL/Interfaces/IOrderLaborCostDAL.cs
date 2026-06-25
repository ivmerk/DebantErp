using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
  public interface IOrderLaborCostDAL : IBaseDAL<OrderLaborCostModel>
  {
    Task<List<OrderLaborCostModel>> GetByOrderId(int orderId);
  }
}
