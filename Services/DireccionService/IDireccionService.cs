using EcommerceStore.DTOs.DireccionDto;

namespace EcommerceStore.Services.DireccionService
{
    public interface IDireccionService
    {
        Task<List<DireccionResponseDto>> GetDireccionesUsuario(int usuarioId);
        Task<DireccionResponseDto> GetDireccionById(int usuarioId, int direccionId);
        Task<DireccionResponseDto> CreateDireccion(int usuarioId, DireccionCreateDto dto);
        Task<DireccionResponseDto> UpdateDireccion(int usuarioId, int direccionId, DireccionUpdateDto dto);
        Task<bool> DeleteDireccion(int usuarioId, int direccionId);
    }
}
