using EcommerceStore.DTOs.CarritoDto;

namespace EcommerceStore.Services.CarritoService
{
    public interface ICarritoService
    {
        Task<CarritoResponseDto> GetCart(int usuarioId);
        Task<CarritoItemResponseDto> GetCartItem(int usuarioId, int varianteId);
        Task<CarritoResponseDto> AddItem(int usuarioId, CarritoAgregarItemDto dto);
        Task<CarritoResponseDto> UpdateItemQuantity(int usuarioId, int varianteId, CarritoActualizarItemDto dto);
        Task<CarritoResponseDto> RemoveItem(int usuarioId, int varianteId);
        Task ClearCart(int usuarioId);
        decimal CalculateTotals(Models.Carrito carrito);
    }
}
