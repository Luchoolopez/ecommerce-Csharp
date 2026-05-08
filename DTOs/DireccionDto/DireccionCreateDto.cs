using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.DireccionDto
{
    public class DireccionCreateDto
    {
        [Required(ErrorMessage = "La calle es obligatoria")]
        public string Calle { get; set; } = null!;

        public string? Numero { get; set; }
        public string? Piso { get; set; }
        public string? Dpto { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; } = null!;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public string Provincia { get; set; } = null!;

        [Required(ErrorMessage = "El código postal es obligatorio")]
        public string CodigoPostal { get; set; } = null!;

        public string Pais { get; set; } = "Argentina";

        public bool EsPrincipal { get; set; } = false;
    }
}
