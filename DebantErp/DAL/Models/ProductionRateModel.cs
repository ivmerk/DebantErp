namespace DebantErp.DAL.Models
{
    public class ProductionRateModel
    {
        public int Id { get; set; }

        public int? ProductionOperationId { get; set; }

        public bool IsActual { get; set; } = true;

        public decimal OperationTimeframe { get; set; }

        public decimal Rate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ProductionOperationModel? ProductionOperation { get; set; }
    }
}
