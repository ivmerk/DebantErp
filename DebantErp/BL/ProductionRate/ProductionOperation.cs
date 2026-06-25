using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.ProductionRate
{
    public class ProductionOperation : IProductionOperation
    {
        private readonly IProductionOperationDAL _operationDAL;

        public ProductionOperation(IProductionOperationDAL operationDAL)
        {
            _operationDAL = operationDAL;
        }

        public async Task<List<ProductionOperationRdo>> GetOperations()
        {
            var operations = await _operationDAL.Get();
            return operations
                .Select(o => new ProductionOperationRdo { Id = o.Id, Name = o.Name })
                .ToList();
        }

        public async Task<ProductionOperationRdo> GetOperation(int id)
        {
            var operation = await _operationDAL.Get(id);
            return new ProductionOperationRdo { Id = operation.Id, Name = operation.Name };
        }

        public async Task<int> Create(CreateUpdateProductionOperationDto dto)
        {
            var model = new ProductionOperationModel { Name = (dto.Name ?? "").Trim() };
            return await _operationDAL.Create(model);
        }

        public async Task<int> Update(int id, CreateUpdateProductionOperationDto dto)
        {
            var operation = await _operationDAL.Get(id);
            if (operation.Id == 0)
            {
                return 0;
            }
            operation.Name = (dto.Name ?? "").Trim();
            return await _operationDAL.Update(operation);
        }

        public async Task<int> Delete(int id)
        {
            return await _operationDAL.Delete(id);
        }
    }
}
