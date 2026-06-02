using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public interface IEmployee
    {
        public Task<int> Create(CreateEmployeeDto dto);
        public Task<int> Update(int id, UpdateEmployeeDto dto);
        public Task<int> Delete(int id);
        public Task<EmployeeRdo> Get(int id);
        public Task<List<EmployeeRdo>> Get();
    }
}
