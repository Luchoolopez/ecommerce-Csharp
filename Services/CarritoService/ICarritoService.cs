using EcommerceStore.DTOs.CarritoDto;

namespace EcommerceStore.Services.CarritoService
{
    public interface ICarritoService
    {
        Task<CarritoResponseDto> GetCart(int usuarioId);
        Task<CarritoItemResponseDto> GetCartItem(int usuarioId, int productoId);
        Task<CarritoResponseDto> AddItem(int usuarioId, CarritoAgregarItemDto dto);
        Task<CarritoResponseDto> UpdateItemQuantity(int usuarioId, int productoId, CarritoActualizarItemDto dto);
        Task<CarritoResponseDto> RemoveItem(int usuarioId, int productoId);
        Task ClearCart(int usuarioId);
        decimal CalculateTotals(Models.Carrito carrito);
    }
}
