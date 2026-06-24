using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IEmployeeSpecialtyAssignmentDAL : IBaseDAL<EmployeeSpecialtyAssignmentModel>
    {
        public Task<List<EmployeeSpecialtyAssignmentModel>> GetByEmployeeId(int employeeId);
        public Task<EmployeeSpecialtyAssignmentModel?> GetByEmployeeAndSpecialty(int employeeId, int specialtyId);
    }
}
