using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateGradeDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Розряд має бути додатним числом")]
        public int Grade { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Денна ставка має бути невід'ємною")]
        public decimal DailyRate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }
    }
}
