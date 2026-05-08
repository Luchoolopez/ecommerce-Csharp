namespace EcommerceStore.DTOs.DireccionDto
{
    public class DireccionUpdateDto
    {
        public string? Calle { get; set; }
        public string? Numero { get; set; }
        public string? Piso { get; set; }
        public string? Dpto { get; set; }
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Pais { get; set; }
        public bool? EsPrincipal { get; set; }
    }
}
