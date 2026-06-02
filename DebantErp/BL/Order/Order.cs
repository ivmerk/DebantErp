using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Order
{
  public class Order : IOrder
  {
    private readonly IOrderDAL _orderDAL;

    public Order(IOrderDAL orderDAL)
    {
      _orderDAL = orderDAL;
    }

    public async Task<int> Create(CreateOrderDto dto)
    {
      if (await _orderDAL.IsNumberExist(dto.Number)) throw new Exception("Order already exist");
      var order = new OrderModel { Number = dto.Number };
      return await _orderDAL.Create(order);
    }

    public async Task<OrderRdo> Get(int id)
    {
      var order = await _orderDAL.Get(id);
      if (order == null) throw new Exception("Order not found");
      var orderRdo = new OrderRdo
      {
        Id = order.Id,
        Number = order.Number,
        IsDeleted = order.IsDeleted,
        CreatedAt = order.CreatedAt,
      };
      return orderRdo;
    }

    public async Task<List<OrderRdo>> Get()
    {
      var orders = await _orderDAL.Get();
      if (orders == null || orders.Count == 0) throw new Exception("Orders not found");
      var ordersRdo = orders
        .Select(o => new OrderRdo
        {
          Id = o.Id,
          Number = o.Number,
          IsDeleted = o.IsDeleted,
          CreatedAt = o.CreatedAt,
        })
        .ToList();
      return ordersRdo;
    }
    public async Task<int> Update(int id, UpdateOrderDto dto)
    {
      var order = await _orderDAL.Get(id);
      if (order == null) throw new Exception("Order not found");
      if (dto.Number != null) order.Number = dto.Number;
      return await _orderDAL.Update(order);
    }
    public async Task<int> Delete(int id)
    {
      var order = await _orderDAL.Get(id);
      if (order == null) throw new Exception("Order not found");
      return await _orderDAL.Delete(id);
    }

  }
}
