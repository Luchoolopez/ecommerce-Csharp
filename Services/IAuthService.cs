using EcommerceStore.DTOs;

namespace EcommerceStore.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UsuarioRegisterDto dto);
        Task<AuthResponseDto> LoginAsync(UsuarioLoginDto dto);
        Task<RefreshTokenRequestDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
    }
}
