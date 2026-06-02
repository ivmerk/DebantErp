using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface ISpecialityDAL : IBaseDAL<SpecialityModel>
    {
        public Task<bool> IsExist(string name);
    }
}
