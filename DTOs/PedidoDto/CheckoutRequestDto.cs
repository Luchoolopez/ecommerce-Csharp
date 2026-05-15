using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.PedidoDto
{
    public class CheckoutRequestDto
    {
        [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
        public int DireccionId { get; set; }

        public string? Notas { get; set; }

        public string? CodigoCupon { get; set; }
    }
}
