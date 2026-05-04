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

        public async Task<CarritoItemResponseDto> GetCartItem(int usuarioId, int productoId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (item == null)
            {
                throw new KeyNotFoundException($"El producto {productoId} no está en el carrito");
            }

            return MapItemToDto(item);
        }

        public async Task<CarritoResponseDto> AddItem(int usuarioId, CarritoAgregarItemDto dto)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == dto.ProductoId && p.Activo);
            if (producto == null)
            {
                throw new KeyNotFoundException("Producto no encontrado o inactivo");
            }

            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += dto.Cantidad;
            }
            else
            {
                carrito.Items.Add(new CarritoItem
                {
                    CarritoId = carrito.Id,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    Producto = producto
                });
            }

            await _context.SaveChangesAsync();
            return MapToDto(carrito);
        }

        public async Task<CarritoResponseDto> UpdateItemQuantity(int usuarioId, int productoId, CarritoActualizarItemDto dto)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (itemExistente == null)
            {
                throw new KeyNotFoundException("El producto no está en el carrito");
            }

            itemExistente.Cantidad = dto.Cantidad;
            await _context.SaveChangesAsync();

            return MapToDto(carrito);
        }

        public async Task<CarritoResponseDto> RemoveItem(int usuarioId, int productoId)
        {
            var carrito = await ObtenerOCrearCarrito(usuarioId);
            var itemExistente = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (itemExistente == null)
            {
                throw new KeyNotFoundException("El producto no está en el carrito");
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
                if (item.Producto != null)
                {
                    decimal precioFinal = item.Producto.PrecioBase * (1 - (item.Producto.Descuento / 100));
                    total += precioFinal * item.Cantidad;
                }
            }
            return total;
        }

        private async Task<Carrito> ObtenerOCrearCarrito(int usuarioId)
        {
            var carrito = await _context.Carritos
                .Include(c => c.Items)
                    .ThenInclude(i => i.Producto)
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
            decimal precioUnitario = item.Producto != null ? (item.Producto.PrecioBase - item.Producto.Descuento) : 0;
            return new CarritoItemResponseDto
            {
                ProductoId = item.ProductoId,
                NombreProducto = item.Producto?.Nombre ?? "Producto Desconocido",
                PrecioUnitario = precioUnitario,
                Cantidad = item.Cantidad,
                Subtotal = precioUnitario * item.Cantidad,
                Imagen = item.Producto?.ImagenPrincipal
            };
        }
    }
}
