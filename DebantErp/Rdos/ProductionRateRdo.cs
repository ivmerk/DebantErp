namespace DebantErp.Rdos
{
    public class ProductionRateRdo
    {
        public int Id { get; set; }
        public int? ProductionOperationId { get; set; }
        public string OperationName { get; set; } = "";
        public string OperationCode { get; set; } = "";
        public int? OperationGrade { get; set; }
        // Дневная ставка действующего разряда операции (из таблицы grades по номеру разряда).
        public decimal? GradeDailyRate { get; set; }

        // Хронометраж операции в секундах.
        public decimal OperationTimeframe { get; set; }

        // Секунд в смене — база для перевода дневной ставки в стоимость за секунду.
        public const int SecondsPerShift = 25500;

        // Стоимость операции за штуку = дневная ставка разряда / секунд в смене × хронометраж.
        public decimal CostPerPiece =>
            (GradeDailyRate ?? 0m) / SecondsPerShift * OperationTimeframe;

        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
