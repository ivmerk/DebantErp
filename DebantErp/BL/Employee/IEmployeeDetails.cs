using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public interface IEmployeeDetails
    {
        Task<EmployeeDetailsRdo> GetEmployeeDetailsByEmployeeId(int employeeId);
        Task<int> UpdateEmployeeDetails(int id, UpdateEmployeeDto dto);
    }
}
