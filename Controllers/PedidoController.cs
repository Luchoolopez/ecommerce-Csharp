using EcommerceStore.DTOs.PedidoDto;
using EcommerceStore.Services.PedidoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        private int ObtenerUsuarioId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                throw new UnauthorizedAccessException("Usuario no autorizado o token inválido");
            }
            return userId;
        }

        private string ObtenerRolUsuario()
        {
            return User.FindFirstValue("rol") ?? "usuario";
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CreateOrder(CheckoutRequestDto dto)
        {
            var pedido = await _pedidoService.CreateOrderAsync(ObtenerUsuarioId(), dto);
            return Ok(new { exito = true, mensaje = "Pedido creado exitosamente", data = pedido });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("manual")]
        public async Task<IActionResult> CreateManualOrder(ManualOrderRequestDto dto)
        {
            var pedido = await _pedidoService.CreateManualOrderAsync(dto);
            return Ok(new { exito = true, mensaje = "Pedido manual creado exitosamente", data = pedido });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var pedido = await _pedidoService.GetOrderByIdAsync(id, ObtenerUsuarioId(), ObtenerRolUsuario());
            return Ok(new { exito = true, mensaje = "Pedido obtenido exitosamente", data = pedido });
        }

        [HttpGet("mis-compras")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var pedidos = await _pedidoService.GetOrdersByUserAsync(ObtenerUsuarioId());
            return Ok(new { exito = true, mensaje = "Historial de compras obtenido exitosamente", data = pedidos });
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDto dto)
        {
            var pedido = await _pedidoService.UpdateOrderStatusAsync(id, dto);
            return Ok(new { exito = true, mensaje = "Estado actualizado exitosamente", data = pedido });
        }
    }
}
