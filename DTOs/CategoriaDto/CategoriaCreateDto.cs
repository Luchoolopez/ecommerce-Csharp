using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.CategoriaDto
{
    public class CategoriaCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = null!;
    }
}
