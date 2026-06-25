using System.Text.Json;
using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class EmployeeDAL : IEmployeeDAL
    {
        public async Task<List<EmployeeModel>> Get()
        {
            var result = await DbHelper.QueryAsync<EmployeeModel>(
                "SELECT * FROM employees WHERE is_actual = true",
                new { }
            );
            return result.ToList();
        }

        public async Task<EmployeeModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<EmployeeModel>(
                "SELECT * FROM employees WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new EmployeeModel();
        }

        public async Task<int> Create(EmployeeModel model)
        {
            string sql =
                "INSERT INTO employees ( first_name, middle_name, last_name) VALUES (@firstName,@middleName, @lastName) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }

        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM employees WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(EmployeeModel model)
        {
            string sql =
                "UPDATE employees SET first_name = @firstName, middle_name = @middleName, last_name = @lastName, updated_at = CAST(@UpdatedAt AS timestamp with time zone) WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE employees SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
