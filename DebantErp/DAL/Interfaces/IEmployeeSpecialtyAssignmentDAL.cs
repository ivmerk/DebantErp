using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IEmployeeSpecialtyAssignmentDAL : IBaseDAL<EmployeeSpecialtyAssignmentModel>
    {
        public Task<List<EmployeeSpecialtyAssignmentModel>> GetByEmployeeId(int employeeId);
    }
}
