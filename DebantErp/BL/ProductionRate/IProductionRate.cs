using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.ProductionRate
{
    public interface IProductionRate
    {
        Task<List<ProductionRateRdo>> GetRates();                  // действующие расценки
        Task<List<ProductionRateRdo>> GetHistory(int operationId); // все версии операции
        Task<int> Create(CreateProductionRateDto dto);             // новая действующая расценка
        Task<int> Change(int id, ChangeProductionRateDto dto);     // новая версия (старая в историю)
        Task<int> Delete(int id);                                  // мягкое удаление
    }
}
