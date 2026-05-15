using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.PedidoDto
{
    public class ManualOrderRequestDto
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        public int UsuarioId { get; set; }

        public int? DireccionId { get; set; }

        public string? Notas { get; set; }

        public string? CodigoCupon { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Debe enviar al menos un ítem")]
        public List<ManualOrderItemDto> Items { get; set; } = new List<ManualOrderItemDto>();
    }
}
