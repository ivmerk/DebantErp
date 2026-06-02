using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public class EmployeeDetails : IEmployeeDetails
    {
        private readonly IEmployeeDetailsDAL _employeeDetailsDAL;

        public EmployeeDetails(IEmployeeDetailsDAL employeeDetailsDAL)
        {
            _employeeDetailsDAL = employeeDetailsDAL;
        }

        public async Task<EmployeeDetailsRdo> GetEmployeeDetailsByEmployeeId(int employeeId)
        {
            var employeeDetails = await _employeeDetailsDAL.GetByEmployeeId(employeeId);

            var employeeDetailsRdo = new EmployeeDetailsRdo
            {
                Id = employeeDetails.Id,
                TaxCode = employeeDetails.TaxCode,
                Address = employeeDetails.Address,
                Email = employeeDetails.Email,
                Phone = employeeDetails.Phone,
                BirthDate = employeeDetails.BirthDate.ToString(),
                Gender = employeeDetails.Gender.ToString(),
                Picture = employeeDetails.Picture,
                EmployeeId = employeeDetails.EmployeeId,
            };
            return employeeDetailsRdo;
        }

        public async Task<int> UpdateEmployeeDetails(int employeeId, UpdateEmployeeDto dto)
        {
            var employeeDetails = await _employeeDetailsDAL.GetByEmployeeId(employeeId);
            if (employeeDetails == null)
            {
                throw new Exception("EmployeeDetails not found");
            }

            if (!string.IsNullOrWhiteSpace(dto?.TaxCode))
                employeeDetails.TaxCode = dto.TaxCode;
            if (!string.IsNullOrWhiteSpace(dto?.Address))
                employeeDetails.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto?.Email))
                employeeDetails.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto?.Phone))
                employeeDetails.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto?.BirthDate))
                employeeDetails.BirthDate = DateTime.Parse(dto.BirthDate);
            if (!string.IsNullOrWhiteSpace(dto?.Gender))
                employeeDetails.Gender = dto.Gender == "male" ? GenderEnum.Male : GenderEnum.Female;

            return await _employeeDetailsDAL.Update(employeeDetails);
        }
    }
}
