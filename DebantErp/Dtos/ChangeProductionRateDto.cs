using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    // Изменение действующей расценки = новая версия (операция остаётся прежней).
    public class ChangeProductionRateDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Норма времени должна быть неотрицательной")]
        public int OperationTimeframe { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ставка должна быть неотрицательной")]
        public decimal Rate { get; set; }
    }
}
