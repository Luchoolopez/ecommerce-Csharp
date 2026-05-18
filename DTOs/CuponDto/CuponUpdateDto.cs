using EcommerceStore.Models;

namespace EcommerceStore.DTOs.CuponDto
{
    public class CuponUpdateDto
    {
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public TipoCupon? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public decimal? MontoMinimo { get; set; }
        public int? UsosMaximos { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? Activo { get; set; }
    }
}
