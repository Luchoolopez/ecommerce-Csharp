using System.ComponentModel.DataAnnotations;
using EcommerceStore.Models;

namespace EcommerceStore.DTOs.CuponDto
{
    public class CuponCreateDto
    {
        [Required(ErrorMessage = "El código del cupón es obligatorio")]
        public string Codigo { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required]
        public TipoCupon Tipo { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor debe ser mayor a 0")]
        public decimal Valor { get; set; }

        public decimal MontoMinimo { get; set; } = 0;

        public int? UsosMaximos { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Activo { get; set; } = true;
    }
}
