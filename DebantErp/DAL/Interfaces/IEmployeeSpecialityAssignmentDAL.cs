using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IEmployeeSpecialityAssignmentDAL : IBaseDAL<EmployeeSpecialityAssignmentModel>
    {
        public Task<List<EmployeeSpecialityAssignmentModel>> GetByEmployeeId(int employeeId);
    }
}
