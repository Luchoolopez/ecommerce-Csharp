using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public enum EstadoPedido
    {
        pendiente,
        confirmado,
        armando,
        enviado,
        entregado,
        cancelado
    }

    public enum ShippingProvider
    {
        andreani,
        correo_argentino
    }

    [Table("pedidos")]
    public class Pedido
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("numero_pedido")]
        public string? NumeroPedido { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Column("direccion_id")]
        public int? DireccionId { get; set; }

        [ForeignKey("DireccionId")]
        public Direccion? Direccion { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("estado")]
        public EstadoPedido Estado { get; set; } = EstadoPedido.pendiente;

        [Column("notas")]
        public string? Notas { get; set; }

        [Column("fecha")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Fecha { get; set; }

        [Column("fecha_envio")]
        public DateTime? FechaEnvio { get; set; }

        [Column("fecha_entrega")]
        public DateTime? FechaEntrega { get; set; }

        [Column("shipping_provider")]
        public ShippingProvider? ShippingProvider { get; set; }

        [Column("shipping_service")]
        public string? ShippingService { get; set; }

        [Column("tracking_number")]
        public string? TrackingNumber { get; set; }

        [Column("shipping_cost")]
        public decimal ShippingCost { get; set; } = 0;

        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}
