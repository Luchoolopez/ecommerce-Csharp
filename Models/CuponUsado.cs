using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    [Table("cupones_usados")]
    public class CuponUsado
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cupon_id")]
        public int CuponId { get; set; }

        [ForeignKey("CuponId")]
        public Cupon? Cupon { get; set; }

        [Column("pedido_id")]
        public int PedidoId { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Column("descuento_aplicado")]
        public decimal DescuentoAplicado { get; set; }

        [Column("fecha_uso")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FechaUso { get; set; }
    }
}
