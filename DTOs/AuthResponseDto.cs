namespace EcommerceStore.DTOs
{
    public class AuthResponseDto
    {
        public UsuarioResponseDto Usuario { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
