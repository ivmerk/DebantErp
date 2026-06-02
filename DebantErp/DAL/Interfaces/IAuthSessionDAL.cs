using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IAuthSessionDAL
    {
        Task<int> Create(AuthSessionModel model);
        Task<int> DeactivateByUserId(int userId);
    }
}
