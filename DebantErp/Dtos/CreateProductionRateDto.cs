using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateProductionRateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть операцію")]
        public int ProductionOperationId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Норма часу має бути додатною")]
        public decimal OperationTimeframe { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ставка має бути невід'ємною")]
        public decimal Rate { get; set; }
    }
}
