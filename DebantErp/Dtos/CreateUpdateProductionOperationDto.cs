using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateUpdateProductionOperationDto
    {
        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; } = null!;
    }
}
