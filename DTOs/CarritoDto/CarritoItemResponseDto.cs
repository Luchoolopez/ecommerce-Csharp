namespace EcommerceStore.DTOs.CarritoDto
{
    public class CarritoItemResponseDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = null!;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public string? Imagen { get; set; }
    }
}
