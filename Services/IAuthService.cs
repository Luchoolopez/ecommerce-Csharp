using EcommerceStore.DTOs;

namespace EcommerceStore.Services
{
    public interface IAuthService
    {
        Task<UsuarioResponseDto> RegisterAsync(UsuarioRegisterDto dto);
    }
}
