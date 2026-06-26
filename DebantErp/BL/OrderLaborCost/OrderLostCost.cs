using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;
namespace DebantErp.BL.OrderLaborCost
{
  public class OrderLostCost : IOrderLaborCost
  {
    private readonly IOrderLaborCostDAL _orderLaborCostDAL;

    public OrderLostCost(IOrderLaborCostDAL orderLaborCostDAL) => _orderLaborCostDAL = orderLaborCostDAL;

    
    public async Task<int> Create(CreateOrderLaborCostDto dto)
    {
      var orderLaborCost = new OrderLaborCostModel
      {
        EmployeeId = dto.EmployeeId,
        ProductionRateId = dto.ProductionRateId,
        Quantity = dto.Quantity,
        OrderId = dto.OrderId,
        
      };
      return await _orderLaborCostDAL.Create(orderLaborCost);
    }

    public async Task<List<OrderLaborCostRdo>> Get() 
    {
      
      var costs = await _orderLaborCostDAL.Get();
      return costs.Select(MapRdo).ToList();
    }

    public async Task<List<OrderLaborCostRdo>> GetByOrder(int orderId)
    {
      var costs = await _orderLaborCostDAL.GetByOrderId(orderId);
      return costs.Select(MapRdo).ToList();
    }

    public async Task<List<OrderLaborCostRdo>> GetByEmployee(int employeeId)
    {
      var costs = await _orderLaborCostDAL.GetByEmployeeId(employeeId);
      return costs.Select(MapRdo).ToList();
    }

    private static OrderLaborCostRdo MapRdo(OrderLaborCostModel c) => new()
    {
      Id = c.Id,
      EmployeeId = c.EmployeeId,
      ProductionRateId = c.ProductionRateId,
      Quantity = c.Quantity,
      OrderId = c.OrderId,
      CreatedAt = c.CreatedAt,
    };

    public async Task<OrderLaborCostRdo> Get(int id) 
    {
      var cost = await _orderLaborCostDAL.Get(id);
      if (cost == null) throw new Exception("Cost not found");
      var costRdo = new OrderLaborCostRdo
      {
        Id = cost.Id,
        EmployeeId = cost.EmployeeId,
        ProductionRateId = cost.ProductionRateId,
        Quantity = cost.Quantity,
        OrderId = cost.OrderId,
        CreatedAt = cost.CreatedAt,
      };
      return costRdo;
    }

    public async Task<int> Update(int id, UpdateOrderLaborCostDto dto) 
    {
      var orderLaborCost = await _orderLaborCostDAL.Get(id);
      if (orderLaborCost == null) throw new Exception("Cost not found");
      if (dto.Quantity != null) orderLaborCost.Quantity = dto.Quantity.Value;
      return await _orderLaborCostDAL.Update(orderLaborCost);
    }

    public async Task<int> Delete(int id)
    {
      var orderLaborCost = await _orderLaborCostDAL.Get(id);
      if (orderLaborCost?.Id == null) throw new Exception("Cost not found");
      return await _orderLaborCostDAL.Delete(id);
    }
  }
}
