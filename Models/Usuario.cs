using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceStore.Models
{
    public enum RolUsuario
    {
        usuario,
        admin
    }

    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = null!;

        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("password")]
        public string Password { get; set; } = null!;

        [Column("rol")]
        public RolUsuario Rol { get; set; } = RolUsuario.usuario;

        [Column("telefono")]
        public string? Telefono { get; set; }

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("fecha_ultimo_acceso")]
        public DateTime? FechaUltimoAcceso { get; set; }

        [Column("fecha_creacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //significa que mysql lo va a generar
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_actualizacion")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] //siginifica que mysql lo va a actualizar solo en cada update
        public DateTime FechaActualizacion { get; set; }

        [Column("reset_password_token")]
        public string? ResetPasswordToken { get; set; }

        [Column("reset_password_expires")]
        public DateTime? ResetPasswordExpires { get; set; }
    }
}
