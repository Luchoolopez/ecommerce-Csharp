using EcommerceStore.Data;
using EcommerceStore.DTOs.PedidoDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.PedidoService
{
    public class PedidoService : IPedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PedidoResponseDto> CreateOrderAsync(int usuarioId, CheckoutRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var carrito = await _context.Carritos
                    .Include(c => c.Items)
                        .ThenInclude(i => i.Variante)
                            .ThenInclude(v => v.Producto)
                    .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

                if (carrito == null || !carrito.Items.Any())
                    throw new InvalidOperationException("El carrito está vacío");

                foreach (var item in carrito.Items)
                {
                    if (item.Variante == null)
                        throw new KeyNotFoundException($"Variante no encontrada para el item {item.Id}");

                    if (item.Variante.Stock < item.Cantidad)
                        throw new InvalidOperationException($"Stock insuficiente para {item.Variante.Producto?.Nombre} - {item.Variante.AtributoVariante}");
                }

                decimal total = 0;
                var detalles = new List<DetallePedido>();

                foreach (var item in carrito.Items)
                {
                    decimal precioFinal = item.Variante!.Producto!.PrecioBase * (1 - (item.Variante.Producto.Descuento / 100));
                    total += precioFinal * item.Cantidad;

                    detalles.Add(new DetallePedido
                    {
                        VarianteId = item.VarianteId,
                        SkuVariante = item.Variante.SkuVariante,
                        NombreProducto = item.Variante.Producto.Nombre,
                        AtributoVariante = item.Variante.AtributoVariante,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = precioFinal,
                        DescuentoAplicado = 0
                    });
                }

                decimal descuentoCupon = await CalcularDescuentoCuponAsync(dto.CodigoCupon, usuarioId, total);
                total -= descuentoCupon;
                if (total < 0) total = 0;

                var pedido = new Pedido
                {
                    NumeroPedido = GenerarNumeroPedido(),
                    UsuarioId = usuarioId,
                    DireccionId = dto.DireccionId,
                    Total = total,
                    Estado = EstadoPedido.confirmado,
                    Notas = dto.Notas,
                    Detalles = detalles
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync(); 

                if (!string.IsNullOrEmpty(dto.CodigoCupon))
                {
                    await RegistrarUsoCuponAsync(dto.CodigoCupon, pedido.Id, usuarioId, descuentoCupon);
                }

                _context.CarritoItems.RemoveRange(carrito.Items);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetOrderByIdAsync(pedido.Id, usuarioId, "usuario");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PedidoResponseDto> CreateManualOrderAsync(ManualOrderRequestDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal total = 0;
                var detalles = new List<DetallePedido>();

                foreach (var itemDto in dto.Items)
                {
                    var variante = await _context.VariantesProducto
                        .Include(v => v.Producto)
                        .FirstOrDefaultAsync(v => v.Id == itemDto.VarianteId);

                    if (variante == null)
                        throw new Exception($"Variante {itemDto.VarianteId} no encontrada");

                    if (variante.Stock < itemDto.Cantidad)
                        throw new Exception($"Stock insuficiente para {variante.Producto?.Nombre} - {variante.AtributoVariante}");

                    decimal precioFinal = variante.Producto!.PrecioBase * (1 - (variante.Producto.Descuento / 100));
                    total += precioFinal * itemDto.Cantidad;

                    detalles.Add(new DetallePedido
                    {
                        VarianteId = variante.Id,
                        SkuVariante = variante.SkuVariante,
                        NombreProducto = variante.Producto.Nombre,
                        AtributoVariante = variante.AtributoVariante,
                        Cantidad = itemDto.Cantidad,
                        PrecioUnitario = precioFinal,
                        DescuentoAplicado = 0
                    });
                }

                decimal descuentoCupon = await CalcularDescuentoCuponAsync(dto.CodigoCupon, dto.UsuarioId, total);
                total -= descuentoCupon;
                if (total < 0) total = 0;

                var pedido = new Pedido
                {
                    NumeroPedido = GenerarNumeroPedido(),
                    UsuarioId = dto.UsuarioId,
                    DireccionId = dto.DireccionId,
                    Total = total,
                    Estado = EstadoPedido.confirmado,
                    Notas = dto.Notas,
                    Detalles = detalles
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(dto.CodigoCupon))
                {
                    await RegistrarUsoCuponAsync(dto.CodigoCupon, pedido.Id, dto.UsuarioId, descuentoCupon);
                }

                await transaction.CommitAsync();
                return await GetOrderByIdAsync(pedido.Id, dto.UsuarioId, "admin");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PedidoResponseDto> GetOrderByIdAsync(int pedidoId, int usuarioId, string rol)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado");

            if (rol != "admin" && pedido.UsuarioId != usuarioId)
                throw new UnauthorizedAccessException("No tienes permiso para ver este pedido");

            return MapToDto(pedido);
        }

        public async Task<List<PedidoResponseDto>> GetOrdersByUserAsync(int usuarioId)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return pedidos.Select(MapToDto).ToList();
        }

        public async Task<PedidoResponseDto> UpdateOrderStatusAsync(int pedidoId, UpdateOrderStatusDto dto)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido no encontrado");

            pedido.Estado = dto.Estado;
            if (dto.TrackingNumber != null) pedido.TrackingNumber = dto.TrackingNumber;
            if (dto.ShippingProvider != null) pedido.ShippingProvider = dto.ShippingProvider;
            if (dto.ShippingService != null) pedido.ShippingService = dto.ShippingService;

            if (dto.Estado == EstadoPedido.enviado && pedido.FechaEnvio == null)
                pedido.FechaEnvio = DateTime.UtcNow;
            if (dto.Estado == EstadoPedido.entregado && pedido.FechaEntrega == null)
                pedido.FechaEntrega = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(pedido);
        }

        private string GenerarNumeroPedido()
        {
            return $"PED-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
        }

        private async Task<decimal> CalcularDescuentoCuponAsync(string? codigo, int usuarioId, decimal total)
        {
            if (string.IsNullOrEmpty(codigo)) return 0;

            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Codigo == codigo && c.Activo);
            if (cupon == null) throw new Exception("Cupón no válido o inactivo");

            if (DateTime.UtcNow < cupon.FechaInicio || DateTime.UtcNow > cupon.FechaFin)
                throw new Exception("El cupón está expirado o aún no es válido");

            if (cupon.MontoMinimo > 0 && total < cupon.MontoMinimo)
                throw new Exception($"El pedido no alcanza el monto mínimo de {cupon.MontoMinimo} para este cupón");

            if (cupon.UsosMaximos.HasValue && cupon.UsosActuales >= cupon.UsosMaximos.Value)
                throw new Exception("El cupón ha alcanzado su límite de usos");

            var usoPrevio = await _context.CuponesUsados.AnyAsync(cu => cu.CuponId == cupon.Id && cu.UsuarioId == usuarioId);
            if (usoPrevio)
                throw new Exception("Ya has utilizado este cupón anteriormente");

            if (cupon.Tipo == TipoCupon.monto_fijo)
                return cupon.Valor;
            else
                return total * (cupon.Valor / 100);
        }

        private async Task RegistrarUsoCuponAsync(string codigo, int pedidoId, int usuarioId, decimal descuentoAplicado)
        {
            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Codigo == codigo);
            if (cupon != null)
            {
                cupon.UsosActuales += 1;
                
                var cuponUsado = new CuponUsado
                {
                    CuponId = cupon.Id,
                    PedidoId = pedidoId,
                    UsuarioId = usuarioId,
                    DescuentoAplicado = descuentoAplicado
                };
                
                _context.CuponesUsados.Add(cuponUsado);
                await _context.SaveChangesAsync();
            }
        }

        private PedidoResponseDto MapToDto(Pedido pedido)
        {
            return new PedidoResponseDto
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                UsuarioId = pedido.UsuarioId,
                DireccionId = pedido.DireccionId,
                Total = pedido.Total,
                Estado = pedido.Estado.ToString(),
                Notas = pedido.Notas,
                Fecha = pedido.Fecha,
                FechaEnvio = pedido.FechaEnvio,
                FechaEntrega = pedido.FechaEntrega,
                ShippingProvider = pedido.ShippingProvider?.ToString(),
                ShippingService = pedido.ShippingService,
                TrackingNumber = pedido.TrackingNumber,
                ShippingCost = pedido.ShippingCost,
                Detalles = pedido.Detalles.Select(d => new DetallePedidoResponseDto
                {
                    VarianteId = d.VarianteId,
                    SkuVariante = d.SkuVariante,
                    NombreProducto = d.NombreProducto,
                    AtributoVariante = d.AtributoVariante,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    DescuentoAplicado = d.DescuentoAplicado,
                    Subtotal = d.PrecioUnitario * d.Cantidad
                }).ToList()
            };
        }
    }
}
