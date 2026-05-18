using EcommerceStore.Models;

namespace EcommerceStore.DTOs.CuponDto
{
    public class CuponResponseDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Tipo { get; set; } = null!; // "porcentaje" o "monto_fijo"
        public decimal Valor { get; set; }
        public decimal MontoMinimo { get; set; }
        public int? UsosMaximos { get; set; }
        public int UsosActuales { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
