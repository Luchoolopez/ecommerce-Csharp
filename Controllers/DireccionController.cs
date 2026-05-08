using EcommerceStore.DTOs.DireccionDto;
using EcommerceStore.Services.DireccionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DireccionController : ControllerBase
    {
        private readonly IDireccionService _direccionService;

        public DireccionController(IDireccionService direccionService)
        {
            _direccionService = direccionService;
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
        public async Task<IActionResult> GetDirecciones()
        {
            var direcciones = await _direccionService.GetDireccionesUsuario(ObtenerUsuarioId());
            return Ok(new { exito = true, mensaje = "Direcciones obtenidas exitosamente", data = direcciones });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDireccionById(int id)
        {
            var direccion = await _direccionService.GetDireccionById(ObtenerUsuarioId(), id);
            return Ok(new { exito = true, mensaje = "Dirección obtenida exitosamente", data = direccion });
        }

        [HttpPost]
        public async Task<IActionResult> CreateDireccion(DireccionCreateDto dto)
        {
            var direccion = await _direccionService.CreateDireccion(ObtenerUsuarioId(), dto);
            return Ok(new { exito = true, mensaje = "Dirección creada exitosamente", data = direccion });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDireccion(int id, DireccionUpdateDto dto)
        {
            var direccion = await _direccionService.UpdateDireccion(ObtenerUsuarioId(), id, dto);
            return Ok(new { exito = true, mensaje = "Dirección actualizada exitosamente", data = direccion });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDireccion(int id)
        {
            await _direccionService.DeleteDireccion(ObtenerUsuarioId(), id);
            return Ok(new { exito = true, mensaje = "Dirección eliminada exitosamente", data = (object)null });
        }
    }
}
