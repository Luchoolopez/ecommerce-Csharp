using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("detalles_pedido")]
    public class DetallePedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Column("variante_id")]
        public int VarianteId { get; set; }

        [ForeignKey("VarianteId")]
        public VarianteProducto? Variante { get; set; }

        [Column("sku_variante")]
        public string? SkuVariante { get; set; }

        [Column("nombre_producto")]
        public string? NombreProducto { get; set; }

        [Column("atributo_variante")]
        public string? AtributoVariante { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("descuento_aplicado")]
        public decimal DescuentoAplicado { get; set; } = 0;
    }
}
