using EcommerceStore.DTOs;
using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Models;

namespace EcommerceStore.Services.UsuarioService
{
    public interface IUsuariosService
    {
        Task<UsuarioResponseDto> GetUser(int userId);
        Task<PagedResponse<UsuarioResponseDto>> GetUsers(int page = 1, int limit = 20);
        Task<UsuarioResponseDto> UpdateUser(int userId, UsuarioUpdateDto usuarioDto);
        Task<bool> ChangePassword(int userId, string newPassword);
        Task<bool> DeleteUser(int userId);
    }
}
