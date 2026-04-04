using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = null!;
    }
}
