using EcommerceStore.DTOs.PedidoDto;

namespace EcommerceStore.Services.PedidoService
{
    public interface IPedidoService
    {
        Task<PedidoResponseDto> CreateOrderAsync(int usuarioId, CheckoutRequestDto dto);
        Task<PedidoResponseDto> CreateManualOrderAsync(ManualOrderRequestDto dto);
        Task<PedidoResponseDto> GetOrderByIdAsync(int pedidoId, int usuarioId, string rol);
        Task<List<PedidoResponseDto>> GetOrdersByUserAsync(int usuarioId);
        Task<PedidoResponseDto> UpdateOrderStatusAsync(int pedidoId, UpdateOrderStatusDto dto);
    }
}
