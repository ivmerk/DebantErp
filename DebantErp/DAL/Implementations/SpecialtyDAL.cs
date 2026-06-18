using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class SpecialtyDAL : ISpecialtyDAL
    {
        public async Task<List<SpecialtyModel>> Get()
        {
            var result = await DbHelper.QueryAsync<SpecialtyModel>(
                "SELECT * FROM specialties",
                new { }
            );
            return result.ToList();
        }

        public async Task<SpecialtyModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<SpecialtyModel>(
                "SELECT * FROM specialties WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new SpecialtyModel();
        }

        public async Task<int> Create(SpecialtyModel model)
        {
            string sql = "INSERT INTO specialties (name) VALUES (@name) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM specialties WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(SpecialtyModel model)
        {
            string sql = "UPDATE specialties SET name = @name WHERE id = @id RETURNING id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE specialties SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> IsExist(string name)
        {
            var result = await DbHelper.QueryAsync<SpecialtyModel>(
                "SELECT * FROM specialties WHERE name = @name",
                new { name }
            );
            if (result.Count() == 0)
                return false;
            return true;
        }
    }
}
