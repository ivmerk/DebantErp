using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IProductionOperationDAL : IBaseDAL<ProductionOperationModel>
    {
        Task<bool> IsCodeExist(string code);
        Task<bool> IsCodeExistForOther(string code, int id);
    }
}
