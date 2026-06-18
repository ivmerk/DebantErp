using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface ISpecialtyDAL : IBaseDAL<SpecialtyModel>
    {
        public Task<bool> IsExist(string name);
    }
}
