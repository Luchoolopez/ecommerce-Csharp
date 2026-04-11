using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.AuthDto
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "El Refresh Token es obligatorio")]
        public string RefreshToken { get; set; } = null!;
    }
}
