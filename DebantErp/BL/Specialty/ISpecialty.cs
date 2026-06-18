using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Specialty
{
    public interface ISpecialty
    {
        public Task<List<SpecialtyRdo>> GetSpecialties();
        public Task<SpecialtyRdo> GetSpecialty(int id);
        public Task<int> Create(CreateUpdateSpecialtyDto dto);
        public Task<int> Update(int id, CreateUpdateSpecialtyDto dto);
        public Task<int> Delete(int id);
    }
}
