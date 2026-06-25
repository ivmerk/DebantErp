using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateProductionRateDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Выберите операцию")]
        public int ProductionOperationId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Норма времени должна быть неотрицательной")]
        public int OperationTimeframe { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ставка должна быть неотрицательной")]
        public decimal Rate { get; set; }
    }
}
