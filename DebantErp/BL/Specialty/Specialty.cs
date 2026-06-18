using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Specialty
{
    public class Specialty : ISpecialty
    {
        private readonly ISpecialtyDAL _specialtyDAL;

        public Specialty(ISpecialtyDAL specialtyDAL)
        {
            _specialtyDAL = specialtyDAL;
        }

        public async Task<List<SpecialtyRdo>> GetSpecialties()
        {
            var specialties = await _specialtyDAL.Get();
            var specialtiesRdos = specialties
                .Select(s => new SpecialtyRdo
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActual = s.IsActual,
                })
                .ToList();
            return specialtiesRdos;
        }

        public async Task<SpecialtyRdo> GetSpecialty(int id)
        {
            var specialty = await _specialtyDAL.Get(id);
            var specialtyRdo = new SpecialtyRdo
            {
                Id = specialty.Id,
                Name = specialty.Name,
                IsActual = specialty.IsActual,
            };
            return specialtyRdo;
        }

        public async Task<int> Create(CreateUpdateSpecialtyDto dto)
        {
            var isExist = await _specialtyDAL.IsExist(dto.Name);
            if (isExist)
            {
                throw new Exception("Specialty already exist");
            }

            var specialty = new SpecialtyModel { Name = dto.Name };
            return await _specialtyDAL.Create(specialty);
        }

        public async Task<int> Update(int id, CreateUpdateSpecialtyDto dto)
        {
            var isExist = await _specialtyDAL.IsExist(dto.Name);
            if (isExist)
            {
                throw new Exception("Specialty already exist");
            }
            var specialty = await _specialtyDAL.Get(id);
            specialty.Name = dto.Name;
            return await _specialtyDAL.Update(specialty);
        }

        public async Task<int> Delete(int id)
        {
            return await _specialtyDAL.Delete(id);
        }
    }
}
