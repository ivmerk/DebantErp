using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
  public class OrderLaborCostDAL : IOrderLaborCostDAL
  {

    public async Task<List<OrderLaborCostModel>> Get()
    {
      var result = await DbHelper.QueryAsync<OrderLaborCostModel>(
          "SELECT * FROM order_labor_costs",
          new { }
      );
      return result.ToList();
    }

    public async Task<OrderLaborCostModel> Get(int id)
    {
      var result = await DbHelper.QueryAsync<OrderLaborCostModel>(
          "SELECT * FROM order_labor_costs WHERE id = @id",
          new { id }
      );
      return result.FirstOrDefault() ?? new OrderLaborCostModel();
    }
    public async Task<int> Create(OrderLaborCostModel model)
    {
      string sql =
          "INSERT INTO order_labor_costs ( employee_id, production_rate_id, quantity, order_id) VALUES (@employeeId, @productionRateId, @quantity, @orderId) RETURNING id";
      return await DbHelper.ExecuteScalarAsync<int>(sql, model);
    }

    public async Task<bool> IsExist(int id)
    {
      string sql = "SELECT EXISTS (SELECT 1 FROM order_labor_costs WHERE id = @Id)";
      return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
    }
    public async Task<int> Update(OrderLaborCostModel model)
    {
      string sql =
          "UPDATE order_labor_costs SET employee_id = @employeeId, production_rate_id = @productionRateId, quantity = @quantity, order_id = @orderId , updated_at = CAST(@UpdatedAt AS timestamp with time zone) WHERE id = @id";
      return await DbHelper.ExecuteAsync(sql, model);
    }

    public async Task<int> Delete(int id)
    {
      string sql = "UPDATE order_labor_costs SET is_deleted = true WHERE id = @id";
      return await 
        DbHelper.ExecuteAsync(sql, new { id });
    }
  }
}
