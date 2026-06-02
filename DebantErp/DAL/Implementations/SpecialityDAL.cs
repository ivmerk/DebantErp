using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class SpecialityDAL : ISpecialityDAL
    {
        public async Task<List<SpecialityModel>> Get()
        {
            var result = await DbHelper.QueryAsync<SpecialityModel>(
                "SELECT * FROM specialities",
                new { }
            );
            return result.ToList();
        }

        public async Task<SpecialityModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<SpecialityModel>(
                "SELECT * FROM specialities WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new SpecialityModel();
        }

        public async Task<int> Create(SpecialityModel model)
        {
            string sql = "INSERT INTO specialities (name) VALUES (@name) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM specialities WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(SpecialityModel model)
        {
            string sql = "UPDATE specialities SET name = @name WHERE id = @id RETURNING id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE specialities SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }

        public async Task<bool> IsExist(string name)
        {
            var result = await DbHelper.QueryAsync<SpecialityModel>(
                "SELECT * FROM specialities WHERE name = @name",
                new { name }
            );
            if (result.Count() == 0)
                return false;
            return true;
        }
    }
}
