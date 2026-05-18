using EcommerceStore.DTOs;
using EcommerceStore.DTOs.ProductoDto;

namespace EcommerceStore.Services.ProductoService
{
    public interface IProductoService
    {
        Task<PagedResponse<ProductoResponseDto>> GetProductos(ProductoFilterDto filters);
        Task<ProductoResponseDto> GetProductoById(int id);
        Task<IEnumerable<ProductoResponseDto>> GetFeaturedProductos(int limit = 10);
        Task<IEnumerable<ProductoResponseDto>> GetNewProductos(int limit = 10);
        Task<ProductoResponseDto> CreateProducto(ProductoCreateDto dto);
        Task<ProductoResponseDto> UpdateProducto(int id, ProductoUpdateDto dto);
        Task<ProductoResponseDto> UploadImagenPrincipal(int id, IFormFile file);
        Task<bool> DeleteProducto(int id);
    }
}
