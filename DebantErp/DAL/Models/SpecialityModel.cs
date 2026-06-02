namespace DebantErp.DAL.Models
{
    public class SpecialityModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActual { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
