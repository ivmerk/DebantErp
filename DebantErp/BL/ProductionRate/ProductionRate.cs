using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.ProductionRate
{
    public class ProductionRate : IProductionRate
    {
        private readonly IProductionRateDAL _rateDAL;
        private readonly IProductionOperationDAL _operationDAL;

        public ProductionRate(IProductionRateDAL rateDAL, IProductionOperationDAL operationDAL)
        {
            _rateDAL = rateDAL;
            _operationDAL = operationDAL;
        }

        public async Task<List<ProductionRateRdo>> GetRates()
        {
            var rates = await _rateDAL.Get(); // действующие (is_actual = true)
            var ops = await Operations();
            return rates.Select(r => Map(r, ops)).ToList();
        }

        public async Task<ProductionRateRdo> GetRate(int id)
        {
            var rate = await _rateDAL.Get(id);
            var ops = await Operations();
            return Map(rate, ops);
        }

        public async Task<List<ProductionRateRdo>> GetHistory(int operationId)
        {
            var all = await _rateDAL.GetByProductionOperationId(operationId);
            var ops = await Operations();
            return all
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => Map(r, ops))
                .ToList();
        }

        public async Task<int> Create(CreateProductionRateDto dto)
        {
            // Одна действующая расценка на операцию: если уже есть — версионируем
            // (старую в историю).
            var active = (await _rateDAL.GetByProductionOperationId(dto.ProductionOperationId))
                .FirstOrDefault(r => r.IsActual);
            if (active != null && active.Id != 0)
            {
                await _rateDAL.Delete(active.Id);
            }

            return await _rateDAL.Create(new ProductionRateModel
            {
                ProductionOperationId = dto.ProductionOperationId,
                OperationTimeframe = dto.OperationTimeframe,
                Rate = dto.Rate,
            });
        }

        public async Task<int> Change(int id, ChangeProductionRateDto dto)
        {
            var old = await _rateDAL.Get(id);
            if (old.Id == 0)
            {
                return 0;
            }

            // Версионирование: старую помечаем неактуальной (уходит в историю),
            // вставляем новую действующую версию по той же операции.
            await _rateDAL.Delete(id);
            return await _rateDAL.Create(new ProductionRateModel
            {
                ProductionOperationId = old.ProductionOperationId,
                OperationTimeframe = dto.OperationTimeframe,
                Rate = dto.Rate,
            });
        }

        public Task<int> Delete(int id) => _rateDAL.Delete(id);

        private async Task<Dictionary<int, ProductionOperationModel>> Operations()
        {
            var ops = await _operationDAL.Get();
            return ops.ToDictionary(o => o.Id);
        }

        private static ProductionRateRdo Map(ProductionRateModel r, Dictionary<int, ProductionOperationModel> ops)
        {
            var opId = r.ProductionOperationId;
            var op = opId.HasValue && ops.TryGetValue(opId.Value, out var o) ? o : null;
            return new ProductionRateRdo
            {
                Id = r.Id,
                ProductionOperationId = opId,
                OperationName = op?.Name ?? $"#{opId}",
                OperationCode = op?.Code ?? "",
                OperationTimeframe = r.OperationTimeframe,
                Rate = r.Rate,
                IsActual = r.IsActual,
                CreatedAt = r.CreatedAt,
            };
        }
    }
}
