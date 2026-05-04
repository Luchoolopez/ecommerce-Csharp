using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.CarritoDto
{
    public class CarritoAgregarItemDto
    {
        [Required(ErrorMessage = "El id del producto es obligatorio")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }
}
