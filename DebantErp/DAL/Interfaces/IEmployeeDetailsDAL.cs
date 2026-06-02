using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IEmployeeDetailsDAL : IBaseDAL<EmployeeDetailsModel>
    {
        public Task<EmployeeDetailsModel> GetByEmployeeId(int employeeId);
    }
}
