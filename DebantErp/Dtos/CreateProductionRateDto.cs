using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateProductionRateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть операцію")]
        public int ProductionOperationId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Хронометраж має бути додатним")]
        public decimal OperationTimeframe { get; set; }
    }
}
