using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("sku")]
        public string? Sku { get; set; } = null!;

        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Column("descripcion")]
        public string? Descripcion { get; set; } = null!;

        [Column("precio_base")]
        public decimal PrecioBase { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; } = 0;

        [Column("peso")] //kg
        public decimal? Peso { get;set; }

        [Column("categoria_id")]
        public int? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        [Column("imagen_principal")]
        public string? ImagenPrincipal { get; set; }

        [Column("es_nuevo")]
        public bool EsNuevo { get; set; } = false;

        [Column("es_destacado")]
        public bool EsDestacado { get; set; } = false;

        [Column("meta_title")]
        public string? MetaTitle { get; set; }

        [Column("meta_description")]
        public string? MetaDescription { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("fecha_creacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaActualizacion { get; set; }

        public List<VarianteProducto> Variantes { get; set; } = new List<VarianteProducto>();
    }
}
