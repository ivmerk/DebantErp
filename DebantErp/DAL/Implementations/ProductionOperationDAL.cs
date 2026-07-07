using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class ProductionOperationDAL : IProductionOperationDAL
    {
        public async Task<List<ProductionOperationModel>> Get()
        {
            var result = await DbHelper.QueryAsync<ProductionOperationModel>(
                "SELECT * FROM production_operations WHERE is_actual = true",
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
            string sql = "INSERT INTO production_operations ( name, code ) VALUES (@name, @code) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM production_operations WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        // Код уникален по всей таблице (в т.ч. среди мягко удалённых) — под стать
        // ограничению uq_production_operations_code.
        public async Task<bool> IsCodeExist(string code)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM production_operations WHERE code = @code)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { code });
        }

        public async Task<bool> IsCodeExistForOther(string code, int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM production_operations WHERE code = @code AND id <> @id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { code, id });
        }

        public async Task<int> Update(ProductionOperationModel model)
        {
            string sql = "UPDATE production_operations SET name = @name, code = @code WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        // Мягкое удаление: запись и связанные расценки сохраняются.
        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE production_operations SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
