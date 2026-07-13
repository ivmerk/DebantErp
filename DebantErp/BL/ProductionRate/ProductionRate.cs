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
        private readonly IGradeDAL _gradeDAL;

        public ProductionRate(IProductionRateDAL rateDAL, IProductionOperationDAL operationDAL, IGradeDAL gradeDAL)
        {
            _rateDAL = rateDAL;
            _operationDAL = operationDAL;
            _gradeDAL = gradeDAL;
        }

        public async Task<List<ProductionRateRdo>> GetRates()
        {
            var rates = await _rateDAL.Get(); // действующие (is_actual = true)
            var ops = await Operations();
            var grades = await Grades();
            return rates.Select(r => Map(r, ops, grades)).ToList();
        }

        public async Task<ProductionRateRdo> GetRate(int id)
        {
            var rate = await _rateDAL.Get(id);
            var ops = await Operations();
            var grades = await Grades();
            return Map(rate, ops, grades);
        }

        public async Task<List<ProductionRateRdo>> GetHistory(int operationId)
        {
            var all = await _rateDAL.GetByProductionOperationId(operationId);
            var ops = await Operations();
            var grades = await Grades();
            return all
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => Map(r, ops, grades))
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
            });
        }

        public Task<int> Delete(int id) => _rateDAL.Delete(id);

        private async Task<Dictionary<int, ProductionOperationModel>> Operations()
        {
            var ops = await _operationDAL.Get();
            return ops.ToDictionary(o => o.Id);
        }

        // Действующая дневная ставка по номеру разряда.
        private async Task<Dictionary<int, decimal>> Grades()
        {
            var grades = await _gradeDAL.Get(); // действующие (is_actual = true)
            return grades
                .GroupBy(g => g.Grade)
                .ToDictionary(g => g.Key, g => g.First().DailyRate);
        }

        private static ProductionRateRdo Map(
            ProductionRateModel r,
            Dictionary<int, ProductionOperationModel> ops,
            Dictionary<int, decimal> grades)
        {
            var opId = r.ProductionOperationId;
            var op = opId.HasValue && ops.TryGetValue(opId.Value, out var o) ? o : null;
            decimal? gradeDailyRate =
                op?.Grade is int grade && grades.TryGetValue(grade, out var rate) ? rate : null;
            return new ProductionRateRdo
            {
                Id = r.Id,
                ProductionOperationId = opId,
                OperationName = op?.Name ?? $"#{opId}",
                OperationCode = op?.Code ?? "",
                OperationGrade = op?.Grade,
                GradeDailyRate = gradeDailyRate,
                OperationTimeframe = r.OperationTimeframe,
                IsActual = r.IsActual,
                CreatedAt = r.CreatedAt,
            };
        }
    }
}
