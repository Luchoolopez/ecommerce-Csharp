using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "El Refresh Token es obligatorio")]
        public string RefreshToken { get; set; } = null!;
    }
}
