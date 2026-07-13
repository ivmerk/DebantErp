namespace DebantErp.DAL.Models
{
    public class ProductionOperationModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public bool IsActual { get; set; } = true;

        public int? Grade { get; set; }   // разряд операции

        public DateTime CreatedAt { get; set; }
    }
}
