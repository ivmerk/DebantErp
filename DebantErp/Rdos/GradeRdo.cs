namespace DebantErp.Rdos
{
    public class GradeRdo
    {
        // Условное число рабочих дней в месяце для расчёта оклада.
        public const int WorkingDaysPerMonth = 21;

        public int Id { get; set; }
        public int Grade { get; set; }
        public decimal DailyRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }

        // Месячный оклад при 21 рабочем дне.
        public decimal MonthlySalary => DailyRate * WorkingDaysPerMonth;
    }
}
