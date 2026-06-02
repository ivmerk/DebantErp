using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Speciality
{
    public interface ISpeciality
    {
        public Task<List<SpecialityRdo>> GetSpecialities();
        public Task<SpecialityRdo> GetSpeciality(int id);
        public Task<int> Create(CreateUpdateSpecialityDto dto);
        public Task<int> Update(int id, CreateUpdateSpecialityDto dto);
        public Task<int> Delete(int id);
    }
}
