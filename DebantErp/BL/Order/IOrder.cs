using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Order
{
  public interface IOrder
  {
    public Task<int> Create(CreateOrderDto dto);
    public Task<OrderRdo> Get(int id);
    public Task<List<OrderRdo>> Get(); 
    public Task<int> Update(int id, UpdateOrderDto dto);
    public Task<int> Delete(int id);
  }
}
