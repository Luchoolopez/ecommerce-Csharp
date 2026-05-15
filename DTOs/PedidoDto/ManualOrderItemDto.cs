using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.PedidoDto
{
    public class ManualOrderItemDto
    {
        [Required]
        public int VarianteId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}
