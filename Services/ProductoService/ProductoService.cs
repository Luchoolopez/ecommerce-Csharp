using EcommerceStore.Data;
using EcommerceStore.DTOs;
using EcommerceStore.DTOs.ProductoDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.ProductoService
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<ProductoResponseDto>> GetProductos(ProductoFilterDto filters)
        {
            var query = _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .AsQueryable();

            if (filters.CategoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == filters.CategoriaId);
            }

            if (!string.IsNullOrEmpty(filters.Busqueda))
            {
                query = query.Where(p => p.Nombre.Contains(filters.Busqueda) || p.Descripcion.Contains(filters.Busqueda));
            }

            var totalItems = await query.CountAsync();

            var productos = await query
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .ToListAsync();

            return new PagedResponse<ProductoResponseDto>
            {
                Items = productos.Select(MapToDto),
                TotalItems = totalItems,
                Page = filters.Page,
                Limit = filters.Limit
            };
        }

        public async Task<ProductoResponseDto> GetProductoById(int id)
        {
            var producto = await _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                throw new KeyNotFoundException($"Producto con id {id} no encontrado");
            }

            return MapToDto(producto);
        }

        public async Task<IEnumerable<ProductoResponseDto>> GetFeaturedProductos(int limit = 10)
        {
            var productos = await _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Where(p => p.EsDestacado == true && p.Activo == true)
                .OrderByDescending(p => p.FechaActualizacion)
                .Take(limit)
                .ToListAsync();

            return productos.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductoResponseDto>> GetNewProductos(int limit = 10)
        {
            var productos = await _context.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Where(p => p.EsNuevo == true && p.Activo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(limit)
                .ToListAsync();

            return productos.Select(MapToDto);
        }

        public async Task<ProductoResponseDto> CreateProducto(ProductoCreateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Sku))
            {
                var skuExiste = await _context.Productos.AnyAsync(p => p.Sku == dto.Sku);
                if (skuExiste) throw new Exception("Ya existe un producto con este SKU");
            }

            if (dto.CategoriaId.HasValue)
            {
                var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == dto.CategoriaId.Value);
                if (!categoriaExiste) throw new Exception("La categoría especificada no existe");
            }

            var producto = new Producto
            {
                Sku = dto.Sku,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                PrecioBase = dto.PrecioBase,
                Descuento = dto.Descuento,
                Peso = dto.Peso,
                CategoriaId = dto.CategoriaId,
                ImagenPrincipal = dto.ImagenPrincipal,
                EsNuevo = dto.EsNuevo,
                EsDestacado = dto.EsDestacado,
                Activo = dto.Activo
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            if (producto.CategoriaId.HasValue)
            {
                producto.Categoria = await _context.Categorias.FindAsync(producto.CategoriaId);
            }

            return MapToDto(producto);
        }

        public async Task<ProductoResponseDto> UpdateProducto(int id, ProductoUpdateDto dto)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                throw new KeyNotFoundException($"Producto con id {id} no encontrado");
            }

            producto.Sku = dto.Sku ?? producto.Sku;
            producto.Nombre = dto.Nombre ?? producto.Nombre;
            producto.Descripcion = dto.Descripcion ?? producto.Descripcion;
            producto.PrecioBase = dto.PrecioBase ?? producto.PrecioBase;
            producto.Descuento = dto.Descuento ?? producto.Descuento;
            producto.Peso = dto.Peso ?? producto.Peso;

            if (dto.CategoriaId.HasValue && dto.CategoriaId != producto.CategoriaId)
            {
                producto.CategoriaId = dto.CategoriaId;
                producto.Categoria = await _context.Categorias.FindAsync(dto.CategoriaId); 
            }

            producto.ImagenPrincipal = dto.ImagenPrincipal ?? producto.ImagenPrincipal;
            producto.EsNuevo = dto.EsNuevo ?? producto.EsNuevo;
            producto.EsDestacado = dto.EsDestacado ?? producto.EsDestacado;
            if (dto.Activo.HasValue)
            {
                producto.Activo = dto.Activo.Value;
            }

            await _context.SaveChangesAsync();
            return MapToDto(producto);
        }

        public async Task<bool> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                throw new KeyNotFoundException($"Producto con id {id} no encontrado");
            }
            producto.Activo = false;
            return true;
        }

        private ProductoResponseDto MapToDto(Producto p)
        {
            return new ProductoResponseDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioBase = p.PrecioBase,
                Descuento = p.Descuento,
                Peso = p.Peso,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre,
                ImagenPrincipal = p.ImagenPrincipal,
                EsNuevo = p.EsNuevo,
                EsDestacado = p.EsDestacado,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaActualizacion = p.FechaActualizacion
            };
        }
    }
}