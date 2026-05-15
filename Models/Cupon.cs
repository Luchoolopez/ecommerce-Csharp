using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public enum TipoCupon
    {
        porcentaje,
        monto_fijo
    }

    [Table("cupones")]
    public class Cupon
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("codigo")]
        public string Codigo { get; set; } = null!;

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("tipo")]
        public TipoCupon Tipo { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("monto_minimo")]
        public decimal MontoMinimo { get; set; } = 0;

        [Column("usos_maximos")]
        public int? UsosMaximos { get; set; }

        [Column("usos_actuales")]
        public int UsosActuales { get; set; } = 0;

        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime FechaFin { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("fecha_creacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; }
    }
}
