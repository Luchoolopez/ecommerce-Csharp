using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("variantes_producto")]
    public class VarianteProducto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Column("atributo_variante")]
        public string AtributoVariante { get; set; } = null!;

        [Column("sku_variante")]
        public string? SkuVariante { get; set; }

        [Column("stock")]
        public int Stock { get; set; } = 0;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("fecha_creacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaActualizacion { get; set; }
    }
}
