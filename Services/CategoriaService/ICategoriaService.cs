using EcommerceStore.DTOs.CategoriaDto;
using EcommerceStore.Models;

namespace EcommerceStore.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponseDto>>GetCategorias();
        Task<CategoriaResponseDto> GetCategoriaById(int categoriaId);
        Task<CategoriaResponseDto> CreateCategoria(CategoriaCreateDto categoria);
        Task<CategoriaResponseDto> UpdateCategoria(int categoriaId, CategoriaUpdateDto categoria);
        Task<bool> DeleteCategoria(int categoriaId);

    }
}
