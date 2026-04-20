using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Models;

namespace EcommerceStore.Services.UsuarioService
{
    public interface IUsuariosService
    {
        Task<UsuarioResponseDto> GetUser(int userId);
        Task<IEnumerable<UsuarioResponseDto>> GetUsers();
        Task<UsuarioResponseDto> UpdateUser(int userId, UsuarioUpdateDto usuarioDto);
        Task<bool> ChangePassword(int userId, string newPassword);
        Task<bool> DeleteUser(int userId);
    }
}
