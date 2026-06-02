using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IProductionRateDAL : IBaseDAL<ProductionRateModel>
    {
        public Task<List<ProductionRateModel>> GetByProductionOperationId(
            int productionOperationId
        );
    }
}
