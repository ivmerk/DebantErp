using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IAuthDAL : IBaseDAL<UserModel>
    {
        Task<UserModel> Get(string email);
    }
}
