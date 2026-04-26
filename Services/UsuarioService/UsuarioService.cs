using EcommerceStore.Data;
using EcommerceStore.DTOs;
using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.UsuarioService
{
    public class UsuarioService : IUsuariosService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioResponseDto> GetUser(int userId)
        {
            var user = await _context.Usuarios.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            var userResponse = new UsuarioResponseDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Email = user.Email,
                Rol = user.Rol.ToString(),
                Telefono = user.Telefono,
                Activo = user.Activo,
                FechaCreacion = user.FechaCreacion
            };
            return userResponse;
        }

        public async Task<PagedResponse<UsuarioResponseDto>> GetUsers(int page = 1, int limit = 20)
        {
            var query = _context.Usuarios.AsQueryable();
            var totalItems = await query.CountAsync(); //cuenta cuantos user hay en la db para que el front sepa cuantas pags hay

            var users = await query
                .Skip((page - 1) * limit) //salta los items de las paginas anteriores
                .Take(limit) //toma solo los items de la pagina actual
                .ToListAsync();

            var userDtos = users.Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol.ToString(),
                Telefono = u.Telefono,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion
            });

            return new PagedResponse<UsuarioResponseDto>
            {
                Items = userDtos,
                TotalItems = totalItems,
                Page = page,
                Limit = limit,
            };


        }

        public async Task<UsuarioResponseDto> UpdateUser(int userId, UsuarioUpdateDto usuarioDto)
        {
            var userToUpdate = await _context.Usuarios.FindAsync(userId);
            if (userToUpdate == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            if (!string.IsNullOrEmpty(usuarioDto.Nombre))
            {
                userToUpdate.Nombre = usuarioDto.Nombre;
            }
            if (!string.IsNullOrEmpty(usuarioDto.Telefono))
            {
                userToUpdate.Telefono = usuarioDto.Telefono;
            }
            if (usuarioDto.Activo.HasValue)
            {
                userToUpdate.Activo = usuarioDto.Activo.Value;
            }

            await _context.SaveChangesAsync();
            var userResponse = new UsuarioResponseDto
            {
                Id = userToUpdate.Id,
                Nombre = userToUpdate.Nombre,
                Email = userToUpdate.Email,
                Rol = userToUpdate.Rol.ToString(),
                Telefono = userToUpdate.Telefono,
                Activo = userToUpdate.Activo,
                FechaCreacion = userToUpdate.FechaCreacion
            };
            return userResponse;

        }

        public async Task<bool> ChangePassword(int userId, string newPassword)
        {
            //hay que implementar mas adelante aca 
            return false;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var userToDelete = await _context.Usuarios.FindAsync(userId);
            if (userToDelete == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            userToDelete.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
