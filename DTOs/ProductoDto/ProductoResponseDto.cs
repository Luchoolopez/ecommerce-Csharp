namespace EcommerceStore.DTOs.ProductoDto
{
    public class ProductoResponseDto
    {
        public int Id { get; set; }
        public string? Sku { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal Descuento { get; set; }
        //Calcula el precio con el descuento aplicado para el frontend
        public decimal PrecioFinal => PrecioBase - (PrecioBase * (Descuento / 100));
        public decimal? Peso { get; set; }
        public int? CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? ImagenPrincipal { get; set; }
        public bool EsNuevo { get; set; }
        public bool EsDestacado { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}