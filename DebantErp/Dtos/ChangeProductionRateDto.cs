using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    // Изменение действующей расценки = новая версия (операция остаётся прежней).
    public class ChangeProductionRateDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Хронометраж має бути додатним")]
        public decimal OperationTimeframe { get; set; }
    }
}
