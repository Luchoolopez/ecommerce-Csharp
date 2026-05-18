using EcommerceStore.DTOs.CuponDto;

namespace EcommerceStore.Services.CuponService
{
    public interface ICuponService
    {
        Task<IEnumerable<CuponResponseDto>> GetCuponesAsync(bool onlyActive = false);
        Task<CuponResponseDto> GetCuponByIdAsync(int id);
        Task<CuponResponseDto> GetCuponByCodigoAsync(string codigo);
        Task<CuponResponseDto> CreateCuponAsync(CuponCreateDto dto);
        Task<CuponResponseDto> UpdateCuponAsync(int id, CuponUpdateDto dto);
        Task<bool> DeleteCuponAsync(int id);
    }
}
