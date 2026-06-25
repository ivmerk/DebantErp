using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.ProductionRate
{
    public interface IProductionOperation
    {
        Task<List<ProductionOperationRdo>> GetOperations();
        Task<ProductionOperationRdo> GetOperation(int id);
        Task<int> Create(CreateUpdateProductionOperationDto dto);
        Task<int> Update(int id, CreateUpdateProductionOperationDto dto);
        Task<int> Delete(int id);
    }
}
