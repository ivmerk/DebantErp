namespace DebantErp.DAL.Models
{
    public class OrderLaborCostModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; } 

        public int ProductionRateId { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public bool IsDeleted { get; set; } = false;
        //public DateTime DateOfStarting { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
