using EcommerceStore.Data;
using EcommerceStore.DTOs.CarritoDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.CarritoService
{
    public class CarritoService : ICarritoService
    {
        private readonly AppDbContext _context;

        public CarritoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarritoResponseDto> GetCart(int usuarioId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            return MapToDto(carrito);
        }

        public async Task<CarritoItemResponseDto> GetCartItem(int usuarioId, int varianteId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var item = carrito.Items.FirstOrDefault(i => i.VarianteId == varianteId);

            if (item == null)
            {
                throw new KeyNotFoundException($"La variante {varianteId} no está en el carrito");
            }

            return MapItemToDto(item);
        }

        public async Task<CarritoResponseDto> AddItem(int usuarioId, CarritoAgregarItemDto dto)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);

            var variante = await _context.VariantesProducto
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(v => v.Id == dto.VarianteId && v.Activo && v.Producto != null && v.Producto.Activo);

            if (variante == null)
            {
                throw new KeyNotFoundException("Variante no encontrada o inactiva");
            }

            var itemExistente = carrito.Items.FirstOrDefault(i => i.VarianteId == dto.VarianteId);
            int cantidadTotalRequerida = dto.Cantidad + (itemExistente?.Cantidad ?? 0);

            if (variante.Stock < cantidadTotalRequerida)
            {
                throw new Exception("No hay stock suficiente para esta variante");
            }

            if (itemExistente != null)
            {
                itemExistente.Cantidad += dto.Cantidad;
            }
            else
            {
                carrito.Items.Add(new CarritoItem
                {
                    CarritoId = carrito.Id,
                    VarianteId = dto.VarianteId,
                    Cantidad = dto.Cantidad,
                    Variante = variante
                });
            }

            await _context.SaveChangesAsync();
            return MapToDto(carrito);
        }

        public async Task<CarritoResponseDto> UpdateItemQuantity(int usuarioId, int varianteId, CarritoActualizarItemDto dto)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.VarianteId == varianteId);

            if (itemExistente == null)
            {
                throw new KeyNotFoundException("La variante no está en el carrito");
            }

            var variante = await _context.VariantesProducto.FindAsync(varianteId);
            if (variante == null || variante.Stock < dto.Cantidad)
            {
                throw new Exception("No hay stock suficiente para actualizar la cantidad solicitada");
            }

            itemExistente.Cantidad = dto.Cantidad;
            await _context.SaveChangesAsync();

            return MapToDto(carrito);
        }

        public async Task<CarritoResponseDto> RemoveItem(int usuarioId, int varianteId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.VarianteId == varianteId);

            if (itemExistente == null)
            {
                throw new KeyNotFoundException("La variante no está en el carrito");
            }

            carrito.Items.Remove(itemExistente);
            _context.CarritoItems.Remove(itemExistente);
            await _context.SaveChangesAsync();

            return MapToDto(carrito);
        }

        public async Task ClearCart(int usuarioId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            if (carrito.Items.Any())
            {
                _context.CarritoItems.RemoveRange(carrito.Items);
                carrito.Items.Clear();
                await _context.SaveChangesAsync();
            }
        }

        public decimal CalculateTotals(Carrito carrito)
        {
            if (carrito == null || carrito.Items == null || !carrito.Items.Any())
                return 0;

            decimal total = 0;
            foreach (var item in carrito.Items)
            {
                if (item.Variante != null && item.Variante.Producto != null)
                {
                    decimal precioFinal = item.Variante.Producto.PrecioBase * (1 - (item.Variante.Producto.Descuento / 100));
                    total += precioFinal * item.Cantidad;
                }
            }
            return total;
        }

        private async Task<Carrito> ObtenerOCrearCarrito(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Variante)
                        .ThenInclude(v => v.Producto)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrito == null)
            {
                carrito = new Carrito { UsuarioId = usuarioId };
                _context.Carritos.Add(carrito);
                await _context.SaveChangesAsync();
            }

            return carrito;
        }

        private CarritoResponseDto MapToDto(Carrito carrito)
        {
            return new CarritoResponseDto
            {
                Id = carrito.Id,
                UsuarioId = carrito.UsuarioId,
                Items = carrito.Items.Select(MapItemToDto).ToList(),
                Total = CalculateTotals(carrito)
            };
        }

        private CarritoItemResponseDto MapItemToDto(CarritoItem item)
        {
            var producto = item.Variante?.Producto;
            decimal precioUnitario = 0;
            
            if (producto != null)
            {
                precioUnitario = producto.PrecioBase * (1 - (producto.Descuento / 100));
            }

            return new CarritoItemResponseDto
            {
                VarianteId = item.VarianteId,
                ProductoId = producto?.Id ?? 0,
                NombreProducto = producto?.Nombre ?? "Producto Desconocido",
                AtributoVariante = item.Variante?.AtributoVariante ?? "N/A",
                PrecioUnitario = precioUnitario,
                Cantidad = item.Cantidad,
                Subtotal = precioUnitario * item.Cantidad,
                Imagen = producto?.ImagenPrincipal
            };
        }
    }
}
