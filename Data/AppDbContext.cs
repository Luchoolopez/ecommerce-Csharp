using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Data
{
    public class AppDbContext : DbContext
    {
        //este constructor es obligatorio, es el que recibe la cadena de conexion de appsettings.json
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        //aca se configura los detalles finos de la db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //en user.model guarda el enum como texto en mysql
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            //avisa a c# que el mail no se puede repetir
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Mapeo de Enums a string para la DB
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ShippingProvider)
                .HasConversion<string>();

            modelBuilder.Entity<Cupon>()
                .Property(c => c.Tipo)
                .HasConversion<string>();
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Producto> Productos { get; set; }
        
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<VarianteProducto> VariantesProducto { get; set; }
        public DbSet<Cupon> Cupones { get; set; }
        public DbSet<CuponUsado> CuponesUsados { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
    }
}
