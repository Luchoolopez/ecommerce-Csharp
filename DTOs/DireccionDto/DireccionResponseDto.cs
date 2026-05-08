namespace EcommerceStore.DTOs.DireccionDto
{
    public class DireccionResponseDto
    {
        public int Id { get; set; }
        public string Calle { get; set; } = null!;
        public string? Numero { get; set; }
        public string? Piso { get; set; }
        public string? Dpto { get; set; }
        public string Ciudad { get; set; } = null!;
        public string Provincia { get; set; } = null!;
        public string CodigoPostal { get; set; } = null!;
        public string Pais { get; set; } = null!;
        public bool EsPrincipal { get; set; }
    }
}
