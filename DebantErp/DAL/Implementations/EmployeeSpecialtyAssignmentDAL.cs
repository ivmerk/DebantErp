using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class EmployeeSpecialtyAssignmentDAL : IEmployeeSpecialtyAssignmentDAL
    {
        public Task<List<EmployeeSpecialtyAssignmentModel>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeSpecialtyAssignmentModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<EmployeeSpecialtyAssignmentModel>(
                "SELECT * FROM employee_specialties WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new EmployeeSpecialtyAssignmentModel();
        }

        public async Task<List<EmployeeSpecialtyAssignmentModel>> GetByEmployeeId(int employeeId)
        {
            var result = await DbHelper.QueryAsync<EmployeeSpecialtyAssignmentModel>(
                "SELECT * FROM employee_specialty_assignments WHERE employee_id = @EmployeeId",
                new { employeeId }
            );
            return result.ToList();
        }

        public async Task<int> Create(EmployeeSpecialtyAssignmentModel model)
        {
            string sql =
                "INSERT INTO employee_specialty_assignments ( employee_id, specialty_id, date_from) VALUES (@EmployeeId, @SpecialtyId, @DateFrom) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM employee_specialty_assignments WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(EmployeeSpecialtyAssignmentModel model)
        {
            string sql =
                "UPDATE employee_specialty_assignments SET employee_id = @EmployeeId, specialty_id = @SpecialtyId, updated_at = CAST(@UpdatedAt AS timestamp with time zone), date_from = @DateFrom WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE employee_specialty_assignments SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
