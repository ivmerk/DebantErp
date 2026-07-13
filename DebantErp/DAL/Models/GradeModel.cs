namespace DebantErp.DAL.Models
{
    public class GradeModel
    {
        public int Id { get; set; }

        public int Grade { get; set; }          // номер разряда

        public decimal DailyRate { get; set; }  // дневная ставка

        public DateTime EffectiveDate { get; set; } // дата введения

        public bool IsActual { get; set; } = true;

        public DateTime CreatedAt { get; set; }
    }
}
