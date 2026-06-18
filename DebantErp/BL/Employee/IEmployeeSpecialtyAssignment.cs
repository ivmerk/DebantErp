using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public interface IEmployeeSpecialtyAssignment
    {
        Task<int> Create(CreateEmployeeAssignmentDto dto);
        Task<int> Delete(int id);
        Task<EmployeeSpecialtyAssignmentRdo> Get(int id);
        Task<List<EmployeeSpecialtyAssignmentRdo>> GetByEmployee(int employeeId);
        Task<int> Update(int id, UpdateEmployeeAssignmentDto dto);
    }
}
