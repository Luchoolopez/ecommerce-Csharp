using System.ComponentModel.DataAnnotations;

namespace EcommerceStore.DTOs.ProductoDto
{
    public class ProductoCreateDto
    {
        public string? Sku { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio base debe ser mayor a 0")]
        public decimal PrecioBase { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal Descuento { get; set; } = 0;

        public decimal? Peso { get; set; }

        public int? CategoriaId { get; set; }

        public string? ImagenPrincipal { get; set; }

        public bool EsNuevo { get; set; } = false;

        public bool EsDestacado { get; set; } = false;

        public string? MetaTitle { get; set; }

        public string? MetaDescription { get; set; }

        public bool Activo { get; set; } = true;
    }
}