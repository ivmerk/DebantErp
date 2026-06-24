using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public class Employee : IEmployee
    {
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IEmployeeDetailsDAL _employeeDetailsDAL;
        private readonly IEmployeeDetails _employeeDetails;

        public Employee(IEmployeeDAL employeeDAL, IEmployeeDetailsDAL employeeDetailsDAL, IEmployeeDetails employeeDetails)
        {
            _employeeDAL = employeeDAL;
            _employeeDetailsDAL = employeeDetailsDAL;
            _employeeDetails = employeeDetails;
        }

        public async Task<List<EmployeeRdo>> Get()
        {
            var employees = await _employeeDAL.Get();

            var employeeRdos = employees
                .Select(e => new EmployeeRdo
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    IsActual = e.IsActual.ToString(),
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                })
                .ToList();
            return employeeRdos;
        }

        public async Task<EmployeeRdo> Get(int id)
        {
            var employee = await _employeeDAL.Get(id);

            var employeeRdo = new EmployeeRdo
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                IsActual = employee.IsActual.ToString(),
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt,
            };
            return employeeRdo;
        }

        public async Task<int> Create(CreateEmployeeDto dto)
        {
            var employee = new EmployeeModel
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
            };
            var id = await _employeeDAL.Create(employee);

            var employeeDetails = new EmployeeDetailsModel
            {
                TaxCode = dto.TaxCode,
                Address = dto.Address,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = DateTime.Parse(dto.BirthDate),
                Gender = dto.Gender == "male" ? GenderEnum.Male : GenderEnum.Female,
                Picture = "",
                EmployeeId = id,
            };
            await _employeeDetailsDAL.Create(employeeDetails);

            return id;
        }

        public async Task<int> Update(int id, UpdateEmployeeDto dto)
        {
            var emplotyee = await _employeeDAL.Get(id);
            if (emplotyee == null || emplotyee.Id == 0)
            {
                return 0;
            }
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                emplotyee.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.MiddleName))
                emplotyee.MiddleName = dto.MiddleName;
            if (!string.IsNullOrWhiteSpace(dto.LastName))
                emplotyee.LastName = dto.LastName;
            var result = await _employeeDAL.Update(emplotyee);

            // Заодно обновляем детали (ИНН, адрес, почта, телефон, дата рождения, пол).
            await _employeeDetails.UpdateEmployeeDetails(id, dto);

            return result;
        }

        public async Task<int> Delete(int id)
        {
            var employee = await _employeeDAL.Get(id);
            if (employee?.Id == null)
            {
                return 0;
            }
            return await _employeeDAL.Delete(id);
        }
    }
}
