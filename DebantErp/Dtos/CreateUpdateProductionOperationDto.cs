using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateUpdateProductionOperationDto
    {
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = null!;
    }
}
