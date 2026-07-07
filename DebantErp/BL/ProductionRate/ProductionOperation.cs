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
                .Select(o => new ProductionOperationRdo { Id = o.Id, Name = o.Name, Code = o.Code })
                .ToList();
        }

        public async Task<ProductionOperationRdo> GetOperation(int id)
        {
            var operation = await _operationDAL.Get(id);
            return new ProductionOperationRdo { Id = operation.Id, Name = operation.Name, Code = operation.Code };
        }

        // Нормализация наименования: первая буква заглавная, остальные строчные,
        // без крайних пробелов (как у специальностей). "ПОШИВ" -> "Пошив".
        private static string Capitalize(string name)
        {
            name = (name ?? "").Trim();
            if (name.Length == 0) return name;
            return char.ToUpperInvariant(name[0]) + name[1..].ToLowerInvariant();
        }

        public async Task<int> Create(CreateUpdateProductionOperationDto dto)
        {
            var code = (dto.Code ?? "").Trim();
            if (await _operationDAL.IsCodeExist(code))
            {
                throw new Exception("Operation code already exist");
            }
            var model = new ProductionOperationModel { Name = Capitalize(dto.Name), Code = code };
            return await _operationDAL.Create(model);
        }

        public async Task<int> Update(int id, CreateUpdateProductionOperationDto dto)
        {
            var operation = await _operationDAL.Get(id);
            if (operation.Id == 0)
            {
                return 0;
            }
            var code = (dto.Code ?? "").Trim();
            // Дубликат кода проверяем среди прочих записей (сама редактируемая не в счёт).
            if (await _operationDAL.IsCodeExistForOther(code, id))
            {
                throw new Exception("Operation code already exist");
            }
            operation.Name = Capitalize(dto.Name);
            operation.Code = code;
            return await _operationDAL.Update(operation);
        }

        public async Task<int> Delete(int id)
        {
            return await _operationDAL.Delete(id);
        }
    }
}
