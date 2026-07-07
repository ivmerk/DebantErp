using System.ComponentModel.DataAnnotations;

namespace DebantErp.Dtos
{
    public class CreateUpdateProductionOperationDto
    {
        [Required(ErrorMessage = "Назва обов'язкова")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Код обов'язковий")]
        [MaxLength(15, ErrorMessage = "Код не може бути довшим за 15 символів")]
        public string Code { get; set; } = null!;
    }
}
