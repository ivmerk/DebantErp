using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    // Изменение действующего разряда = новая версия (номер разряда прежний).
    public class ChangeGradeDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "Денна ставка має бути невід'ємною")]
        public decimal DailyRate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }
    }
}
