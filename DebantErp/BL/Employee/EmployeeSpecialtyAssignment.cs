using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Employee
{
    public class EmployeeSpecialtyAssignment : IEmployeeSpecialtyAssignment
    {
        private readonly IEmployeeSpecialtyAssignmentDAL _employeeSpecialtyAssignmentDAL;

        public EmployeeSpecialtyAssignment(
            IEmployeeSpecialtyAssignmentDAL employeeSpecialtyAssignmentDAL
        )
        {
            _employeeSpecialtyAssignmentDAL = employeeSpecialtyAssignmentDAL;
        }

        public async Task<int> Create(CreateEmployeeAssignmentDto dto)
        {
            var dateFrom = DateTime.Parse(dto.DateFrom);

            // unique(employee_id, specialty_id): если связь уже была (в т.ч. мягко
            // снятая) — реактивируем её, а не создаём дубликат.
            var existing = await _employeeSpecialtyAssignmentDAL
                .GetByEmployeeAndSpecialty(dto.EmployeeId, dto.SpecialtyId);
            if (existing != null && existing.Id != 0)
            {
                existing.IsActual = true;
                existing.DateFrom = dateFrom;
                return await _employeeSpecialtyAssignmentDAL.Update(existing);
            }

            var model = new EmployeeSpecialtyAssignmentModel
            {
                EmployeeId = dto.EmployeeId,
                SpecialtyId = dto.SpecialtyId,
                DateFrom = dateFrom,
            };
            return await _employeeSpecialtyAssignmentDAL.Create(model);
        }


        public async Task<EmployeeSpecialtyAssignmentRdo> Get(int id)
        {
            var model = await _employeeSpecialtyAssignmentDAL.Get(id);
            var rdo = new EmployeeSpecialtyAssignmentRdo
            {
                Id = model.Id,
                EmployeeId = model.EmployeeId,
                SpecialtyId = model.SpecialtyId,
                IsActual = model.IsActual,
                DateFrom = model.DateFrom,
            };
            return rdo;
        }

        public async Task<List<EmployeeSpecialtyAssignmentRdo>> GetByEmployee(int employeeId)
        {
            var models = await _employeeSpecialtyAssignmentDAL.GetByEmployeeId(employeeId);
            var rdos = new List<EmployeeSpecialtyAssignmentRdo>();
            foreach (var model in models)
            {
                var rdo = new EmployeeSpecialtyAssignmentRdo
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    SpecialtyId = model.SpecialtyId,
                IsActual = model.IsActual,
                    DateFrom = model.DateFrom,
                };
                rdos.Add(rdo);
            }
            return rdos;
        }

        public async Task<int> Update(int id, UpdateEmployeeAssignmentDto dto)
        {
            var assignment = await _employeeSpecialtyAssignmentDAL.Get(id);
            if (assignment == null)
            {
                throw new Exception("EmployeeSpecialtyAssigment not found");
            }
            if (dto.EmployeeId != null)
            {
                assignment.EmployeeId = dto.EmployeeId.Value;
            }

            if (dto.SpecialtyId != null)
            {
                assignment.SpecialtyId = dto.SpecialtyId.Value;
            }

            if (
                !string.IsNullOrWhiteSpace(dto.DateFrom)
                && DateTime.TryParse(dto.DateFrom, out DateTime dateFrom)
            )
            {
                assignment.DateFrom = dateFrom;
            }
            return await _employeeSpecialtyAssignmentDAL.Update(assignment);
        }

        public Task<int> Delete(int id) => _employeeSpecialtyAssignmentDAL.Delete(id);
    }
}
