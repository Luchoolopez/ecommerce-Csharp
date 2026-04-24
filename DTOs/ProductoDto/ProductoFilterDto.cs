namespace EcommerceStore.DTOs.ProductoDto
{
    public class ProductoFilterDto
    {
        public int? CategoriaId { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? Busqueda { get; set; }
        public bool? EsNuevo { get; set; }
        public bool? EsDestacado { get; set; }
        public bool? ConDescuento { get; set; }
        public bool Activo { get; set; } = true;

        // Paginación
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 20;
        public string OrderBy { get; set; } = "fecha_creacion";
        public string OrderDir { get; set; } = "DESC";
    }
}