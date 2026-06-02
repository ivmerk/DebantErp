using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class ProductionRateDAL : IProductionRateDAL
    {
        public async Task<List<ProductionRateModel>> Get()
        {
            var result = await DbHelper.QueryAsync<ProductionRateModel>(
                "SELECT * FROM production_rates",
                new { }
            );
            return result.ToList();
        }

        public async Task<ProductionRateModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<ProductionRateModel>(
                "SELECT * FROM production_rates WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new ProductionRateModel();
        }

        public async Task<List<ProductionRateModel>> GetByProductionOperationId(
            int productionOperationId
        )
        {
            var result = await DbHelper.QueryAsync<ProductionRateModel>(
                "SELECT * FROM production_rates WHERE production_operation_id = @productionOperationId",
                new { productionOperationId }
            );
            return result.ToList();
        }

        public async Task<int> Create(ProductionRateModel model)
        {
            string sql =
                "INSERT INTO production_rates (production_operation_id, operation_timeframe, rate) VALUES (@productionOperationId, @operationTimeframe, @rate) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM production_rates WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(ProductionRateModel model)
        {
            string sql =
                "UPDATE production_rates SET production_operation_id = @productionOperationId, operation_timeframe = @operationTimeframe, rate = @rate, updated_at = CAST(@UpdatedAt AS timestamp with time zone) WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE production_rates SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
