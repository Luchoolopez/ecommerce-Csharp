using EcommerceStore.DTOs;
using EcommerceStore.DTOs.CategoriaDto;
using EcommerceStore.Models;

namespace EcommerceStore.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<PagedResponse<CategoriaResponseDto>>GetCategorias(int page = 1, int limit = 100);
        Task<CategoriaResponseDto> GetCategoriaById(int categoriaId);
        Task<CategoriaResponseDto> CreateCategoria(CategoriaCreateDto categoria);
        Task<CategoriaResponseDto> UpdateCategoria(int categoriaId, CategoriaUpdateDto categoria);
        Task<bool> DeleteCategoria(int categoriaId);

    }
}
