using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateUpdateSpecialityDto
    {
        [Required()]
        public string Name { get; set; } = null!;
    }
}
