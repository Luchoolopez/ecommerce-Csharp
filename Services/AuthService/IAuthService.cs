using EcommerceStore.DTOs.AuthDto;
using EcommerceStore.DTOs.UsuarioDto;

namespace EcommerceStore.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UsuarioRegisterDto dto);
        Task<AuthResponseDto> LoginAsync(UsuarioLoginDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
        Task<UsuarioResponseDto> GetMyProfile(string userId);
        Task Logout(string userId); 
    }
}
