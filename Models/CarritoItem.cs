using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("carrito_items")]
    public class CarritoItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("carrito_id")]
        public int CarritoId { get; set; }

        [ForeignKey("CarritoId")]
        public Carrito? Carrito { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }
    }
}
