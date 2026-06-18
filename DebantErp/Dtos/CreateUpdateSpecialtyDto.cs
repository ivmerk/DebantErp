using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateUpdateSpecialtyDto
    {
        [Required()]
        public string Name { get; set; } = null!;
    }
}
