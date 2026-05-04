using EcommerceStore.DTOs.CarritoDto;
using EcommerceStore.Services.CarritoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protege todo el carrito, solo usuarios logueados pueden entrar
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
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

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var carrito = await _carritoService.GetCart(ObtenerUsuarioId());
            return Ok(new { exito = true, mensaje = "Carrito obtenido exitosamente", data = carrito });
        }

        [HttpGet("items/{productoId}")]
        public async Task<IActionResult> GetCartItem(int productoId)
        {
            var item = await _carritoService.GetCartItem(ObtenerUsuarioId(), productoId);
            return Ok(new { exito = true, mensaje = "Ítem obtenido exitosamente", data = item });
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(CarritoAgregarItemDto dto)
        {
            var carrito = await _carritoService.AddItem(ObtenerUsuarioId(), dto);
            return Ok(new { exito = true, mensaje = "Producto agregado al carrito", data = carrito });
        }

        [HttpPut("items/{productoId}")]
        public async Task<IActionResult> UpdateItemQuantity(int productoId, CarritoActualizarItemDto dto)
        {
            var carrito = await _carritoService.UpdateItemQuantity(ObtenerUsuarioId(), productoId, dto);
            return Ok(new { exito = true, mensaje = "Cantidad actualizada exitosamente", data = carrito });
        }

        [HttpDelete("items/{productoId}")]
        public async Task<IActionResult> RemoveItem(int productoId)
        {
            var carrito = await _carritoService.RemoveItem(ObtenerUsuarioId(), productoId);
            return Ok(new { exito = true, mensaje = "Producto eliminado del carrito", data = carrito });
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _carritoService.ClearCart(ObtenerUsuarioId());
            return Ok(new { exito = true, mensaje = "Carrito vaciado exitosamente", data = (object)null });
        }
    }
}