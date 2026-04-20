using EcommerceStore.Data;
using EcommerceStore.DTOs.CategoriaDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        private readonly AppDbContext _context;

        public CategoriaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaResponseDto>> GetCategorias()
        {
            var categorias = await _context.Categorias.ToListAsync();

            return categorias.Select(c => new CategoriaResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = c.Activo,
                FechaCreacion = c.FechaCreacion
            });
        }

        public async Task<CategoriaResponseDto> GetCategoriaById(int categoriaId)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaId);
            if (categoria == null)
            {
                throw new Exception("Categoria no encontrada");
            }
            var categoriaResponse = new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
            return categoriaResponse;
        }

        public async Task<CategoriaResponseDto> CreateCategoria(CategoriaCreateDto categoria)
        {
            var newCategoria = new Categoria
            {
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Categorias.Add(newCategoria);
            await _context.SaveChangesAsync();

            var categoriaResponse = new CategoriaResponseDto
            {
                Nombre = newCategoria.Nombre,
                Descripcion = newCategoria.Descripcion,
                Activo = newCategoria.Activo,
                FechaCreacion = newCategoria.FechaCreacion
            };

            return categoriaResponse;
        }

        public async Task<CategoriaResponseDto> UpdateCategoria(int categoriaId, CategoriaUpdateDto categoria)
        {
            var categoriaToUpdate = await _context.Categorias.FindAsync(categoriaId);
            if (categoriaToUpdate == null)
            {
                throw new Exception("Categoria no encontrada");
            }

            if (!string.IsNullOrEmpty(categoria.Nombre))
            {
                categoriaToUpdate.Nombre = categoria.Nombre;
            }
            if (!string.IsNullOrEmpty(categoria.Descripcion))
            {
                categoriaToUpdate.Descripcion = categoria.Descripcion;
            }
            if (categoria.Activo.HasValue)
            {
                categoriaToUpdate.Activo = categoria.Activo.Value;
            }

            await _context.SaveChangesAsync();
            var categoriaResponse = new CategoriaResponseDto
            {
                Nombre = categoriaToUpdate.Nombre,
                Descripcion = categoriaToUpdate.Descripcion,
                Activo = categoriaToUpdate.Activo,
                FechaCreacion = categoriaToUpdate.FechaCreacion
            };
            return categoriaResponse;
        }

        public async Task<bool> DeleteCategoria(int categoriaId)
        {
            var categoriaToDelete = await _context.Categorias.FindAsync(categoriaId);
            if (categoriaToDelete == null)
            {
                throw new Exception("Categoria no encontrada");
            }
            categoriaToDelete.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
