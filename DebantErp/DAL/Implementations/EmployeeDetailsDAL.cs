using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class EmployeeDetailsDAL : IEmployeeDetailsDAL
    {
        public Task<List<EmployeeDetailsModel>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeDetailsModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<EmployeeDetailsModel>(
                "SELECT * FROM employees_details WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new EmployeeDetailsModel();
        }

        public async Task<EmployeeDetailsModel> GetByEmployeeId(int employeeId)
        {
            var result = await DbHelper.QueryAsync<EmployeeDetailsModel>(
                "SELECT * FROM employees_details WHERE employee_id = @EmployeeId",
                new { employeeId }
            );
            return result.FirstOrDefault() ?? new EmployeeDetailsModel();
        }

        public async Task<int> Create(EmployeeDetailsModel model)
        {
            string sql =
                "INSERT INTO employees_details (tax_code, address, email, phone, birth_date, gender, picture, employee_id) VALUES (@TaxCode, @Address, @Email, @Phone, @BirthDate, @Gender, @Picture, @EmployeeId) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM employee_details WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(EmployeeDetailsModel model)
        {
            string sql =
                "UPDATE employees_details SET tax_code = @TaxCode, address = @Address, email = @Email, phone = @Phone, birth_date = @BirthDate, gender = @Gender, picture = @Picture, updated_at = CAST(@UpdatedAt AS timestamp with time zone) WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
