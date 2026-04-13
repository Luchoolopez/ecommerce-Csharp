using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.CategoriaDto
{
    public class CategoriaUpdateDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public bool? Activo { get; set; }
    }
}
