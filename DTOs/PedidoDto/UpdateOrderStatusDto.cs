using EcommerceStore.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.PedidoDto
{
    public class UpdateOrderStatusDto
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        public EstadoPedido Estado { get; set; }

        public string? TrackingNumber { get; set; }

        public ShippingProvider? ShippingProvider { get; set; }

        public string? ShippingService { get; set; }
    }
}
