namespace EcommerceStore.DTOs.PedidoDto
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public string? NumeroPedido { get; set; }
        public int UsuarioId { get; set; }
        public int? DireccionId { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = null!;
        public string? Notas { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? ShippingProvider { get; set; }
        public string? ShippingService { get; set; }
        public string? TrackingNumber { get; set; }
        public decimal ShippingCost { get; set; }
        
        public List<DetallePedidoResponseDto> Detalles { get; set; } = new List<DetallePedidoResponseDto>();
    }
}
