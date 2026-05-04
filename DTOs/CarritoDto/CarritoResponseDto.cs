namespace EcommerceStore.DTOs.CarritoDto
{
    public class CarritoResponseDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public List<CarritoItemResponseDto> Items { get; set; } = new List<CarritoItemResponseDto>();
        public decimal Total { get; set; }
    }
}
