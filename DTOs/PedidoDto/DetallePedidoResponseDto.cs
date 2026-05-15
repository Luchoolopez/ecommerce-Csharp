namespace EcommerceStore.DTOs.PedidoDto
{
    public class DetallePedidoResponseDto
    {
        public int VarianteId { get; set; }
        public string? SkuVariante { get; set; }
        public string? NombreProducto { get; set; }
        public string? AtributoVariante { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal Subtotal { get; set; }
    }
}
