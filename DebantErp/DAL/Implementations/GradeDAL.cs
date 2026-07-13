using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class GradeDAL : IGradeDAL
    {
        public async Task<List<GradeModel>> Get()
        {
            var result = await DbHelper.QueryAsync<GradeModel>(
                "SELECT * FROM grades WHERE is_actual = true",
                new { }
            );
            return result.ToList();
        }

        public async Task<GradeModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<GradeModel>(
                "SELECT * FROM grades WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new GradeModel();
        }

        public async Task<List<GradeModel>> GetByGrade(int grade)
        {
            var result = await DbHelper.QueryAsync<GradeModel>(
                "SELECT * FROM grades WHERE grade = @grade",
                new { grade }
            );
            return result.ToList();
        }

        public async Task<int> Create(GradeModel model)
        {
            string sql =
                "INSERT INTO grades (grade, daily_rate, effective_date) VALUES (@Grade, @DailyRate, CAST(@EffectiveDate AS date)) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM grades WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(GradeModel model)
        {
            string sql =
                "UPDATE grades SET grade = @Grade, daily_rate = @DailyRate, effective_date = CAST(@EffectiveDate AS date) WHERE id = @Id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        // Мягкое удаление / отправка версии в историю.
        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE grades SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
