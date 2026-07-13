namespace DebantErp.Rdos
{
    public class ProductionRateRdo
    {
        public int Id { get; set; }
        public int? ProductionOperationId { get; set; }
        public string OperationName { get; set; } = "";
        public string OperationCode { get; set; } = "";
        public int? OperationGrade { get; set; }
        // Данные действующего разряда операции (подтянуты из таблицы grades по номеру разряда).
        public decimal? GradeDailyRate { get; set; }
        public decimal? GradeMonthlySalary =>
            GradeDailyRate.HasValue ? GradeDailyRate.Value * GradeRdo.WorkingDaysPerMonth : null;
        public decimal OperationTimeframe { get; set; }
        public decimal Rate { get; set; }
        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
