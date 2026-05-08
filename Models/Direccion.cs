using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("direcciones")]
    public class Direccion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Column("calle")]
        public string Calle { get; set; } = null!;

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("piso")]
        public string? Piso { get; set; }

        [Column("dpto")]
        public string? Dpto { get; set; }

        [Column("ciudad")]
        public string Ciudad { get; set; } = null!;

        [Column("provincia")]
        public string Provincia { get; set; } = null!;

        [Column("codigo_postal")]
        public string CodigoPostal { get; set; } = null!;

        [Column("pais")]
        public string Pais { get; set; } = "Argentina";

        [Column("es_principal")]
        public bool EsPrincipal { get; set; } = false;
    }
}
