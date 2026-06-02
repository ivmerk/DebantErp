using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public class EmployeeSpecialityAssignmentDAL : IEmployeeSpecialityAssignmentDAL
    {
        public Task<List<EmployeeSpecialityAssignmentModel>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeSpecialityAssignmentModel> Get(int id)
        {
            var result = await DbHelper.QueryAsync<EmployeeSpecialityAssignmentModel>(
                "SELECT * FROM employee_specialities WHERE id = @id",
                new { id }
            );
            return result.FirstOrDefault() ?? new EmployeeSpecialityAssignmentModel();
        }

        public async Task<List<EmployeeSpecialityAssignmentModel>> GetByEmployeeId(int employeeId)
        {
            var result = await DbHelper.QueryAsync<EmployeeSpecialityAssignmentModel>(
                "SELECT * FROM employee_speciality_assignments WHERE employee_id = @EmployeeId",
                new { employeeId }
            );
            return result.ToList();
        }

        public async Task<int> Create(EmployeeSpecialityAssignmentModel model)
        {
            string sql =
                "INSERT INTO employee_speciality_assignments ( employee_id, speciality_id, date_from) VALUES (@EmployeeId, @SpecialityId, @DateFrom) RETURNING id";
            return await DbHelper.ExecuteScalarAsync<int>(sql, model);
        }
        public async Task<bool> IsExist(int id)
        {
            string sql = "SELECT EXISTS (SELECT 1 FROM employee_speciality_assignments WHERE id = @Id)";
            return await DbHelper.ExecuteScalarAsync<bool>(sql, new { id });
        }

        public async Task<int> Update(EmployeeSpecialityAssignmentModel model)
        {
            string sql =
                "UPDATE employee_speciality_assignments SET employee_id = @EmployeeId, speciality_id = @SpecialityId, updated_at = CAST(@UpdatedAt AS timestamp with time zone), date_from = @DateFrom WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, model);
        }

        public async Task<int> Delete(int id)
        {
            string sql = "UPDATE employee_speciality_assignments SET is_actual = false WHERE id = @id";
            return await DbHelper.ExecuteAsync(sql, new { id });
        }
    }
}
