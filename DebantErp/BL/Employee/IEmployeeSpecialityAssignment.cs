using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public interface IEmployeeSpecialityAssignment
    {
        Task<int> Create(CreateEmployeeAssignmentDto dto);
        Task<int> Delete(int id);
        Task<EmployeeSpecialityAssignmentRdo> Get(int id);
        Task<List<EmployeeSpecialityAssignmentRdo>> GetByEmployee(int employeeId);
        Task<int> Update(int id, UpdateEmployeeAssignmentDto dto);
    }
}
