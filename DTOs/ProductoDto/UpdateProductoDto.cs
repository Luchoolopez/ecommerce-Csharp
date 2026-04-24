namespace EcommerceStore.DTOs.ProductoDto
{
    public class ProductoUpdateDto
    {
        public string? Sku { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PrecioBase { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? Peso { get; set; }
        public int? CategoriaId { get; set; }
        public string? ImagenPrincipal { get; set; }
        public bool? EsNuevo { get; set; }
        public bool? EsDestacado { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public bool? Activo { get; set; }
    }
}