namespace DebantErp.Rdos
{
    public class ProductionRateRdo
    {
        public int Id { get; set; }
        public int? ProductionOperationId { get; set; }
        public string OperationName { get; set; } = "";
        public decimal OperationTimeframe { get; set; }
        public decimal Rate { get; set; }
        public bool IsActual { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
