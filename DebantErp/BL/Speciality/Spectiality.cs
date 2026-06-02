using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Speciality
{
    public class Speciality : ISpeciality
    {
        private readonly ISpecialityDAL _specialityDAL;

        public Speciality(ISpecialityDAL specialityDAL)
        {
            _specialityDAL = specialityDAL;
        }

        public async Task<List<SpecialityRdo>> GetSpecialities()
        {
            var specialities = await _specialityDAL.Get();
            var specialitiesRdos = specialities
                .Select(s => new SpecialityRdo
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActual = s.IsActual,
                })
                .ToList();
            return specialitiesRdos;
        }

        public async Task<SpecialityRdo> GetSpeciality(int id)
        {
            var speciality = await _specialityDAL.Get(id);
            var specialityRdo = new SpecialityRdo
            {
                Id = speciality.Id,
                Name = speciality.Name,
                IsActual = speciality.IsActual,
            };
            return specialityRdo;
        }

        public async Task<int> Create(CreateUpdateSpecialityDto dto)
        {
            var isExist = await _specialityDAL.IsExist(dto.Name);
            if (isExist)
            {
                throw new Exception("Speciality already exist");
            }

            var speciality = new SpecialityModel { Name = dto.Name };
            return await _specialityDAL.Create(speciality);
        }

        public async Task<int> Update(int id, CreateUpdateSpecialityDto dto)
        {
            var isExist = await _specialityDAL.IsExist(dto.Name);
            if (isExist)
            {
                throw new Exception("Speciality already exist");
            }
            var speciality = await _specialityDAL.Get(id);
            speciality.Name = dto.Name;
            return await _specialityDAL.Update(speciality);
        }

        public async Task<int> Delete(int id)
        {
            return await _specialityDAL.Delete(id);
        }
    }
}
