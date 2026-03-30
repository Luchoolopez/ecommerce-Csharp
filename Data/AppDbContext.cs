using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Data
{
    public class AppDbContext : DbContext
    {
        //este constructor es obligatorio, es el que recibe la cadena de conexion de appsettings.json
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        //esto representa la tabla "usuarios" en la db 
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
