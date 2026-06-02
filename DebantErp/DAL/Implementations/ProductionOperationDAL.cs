using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class ProductionOperationDAL : IProductionOperationDAL
    {
        public async Task<List<ProductionOperationModel>> Get()
        {
            var result = await DbHelper.QueryAsync<ProductionOperationModel>(
                "SELECT * FROM production_operations",
                new { }
            );
            return result.ToList();
        }

        public async Task<ProductionOperationModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<ProductionOperationModel>(
                "SELECT * FROM production_operations WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new ProductionOperationModel();
        }

        public async Task<int> Create(ProductionOperationModel model)
        {
            string sql = "INSERT INTO production_operations ( name ) VALUES (@name) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM production_operations WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public Task<int> Update(ProductionOperationModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
