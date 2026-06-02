namespace DebantErp.DAL.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
