using EcommerceStore.Models;

namespace EcommerceStore.DTOs.UsuarioDto
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public RolUsuario Rol { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion {  get; set; }
    }
}
