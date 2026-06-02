using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class OrderDAL : IOrderDAL
    {
        public async Task<List<OrderModel>> Get()
        {
            var result = await DbHelper.QueryAsync<OrderModel>("SELECT * FROM orders", new { });
            return result.ToList();
        }

        public async Task<OrderModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<OrderModel>(
                "SELECT * FROM orders WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new OrderModel();
        }

        public async Task<int> Create(OrderModel model)
        {
            string sql = "INSERT INTO orders (number) VALUES (@number) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM orders WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<bool> IsNumberExist(string name)
        {
            var result = await DbHelper.QueryAsync<OrderModel>(
                "SELECT * FROM orders WHERE number = @number",
                new { number = name }
            );
            if (result.Count() == 0)
                return false;
            return true;
        }
        public async Task<int> Update(OrderModel model)
        {
            string sql = "UPDATE orders SET number = @number WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public Task<int> Delete(int id)
        {
          return DbHelper.ExecuteAsync("UPDATE orders SET is_deleted = true WHERE id = @id", new { id });
        }
    }
}
