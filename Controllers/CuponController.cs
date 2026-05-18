using EcommerceStore.DTOs.CuponDto;
using EcommerceStore.Services.CuponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuponController : ControllerBase
    {
        private readonly ICuponService _cuponService;

        public CuponController(ICuponService cuponService)
        {
            _cuponService = cuponService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetCupones([FromQuery] bool onlyActive = false)
        {
            var cupones = await _cuponService.GetCuponesAsync(onlyActive);
            return Ok(new { exito = true, data = cupones });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCuponById(int id)
        {
            var cupon = await _cuponService.GetCuponByIdAsync(id);
            return Ok(new { exito = true, data = cupon });
        }

        // Endpoint público para que los usuarios validen su cupón en el checkout
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetCuponByCodigo(string codigo)
        {
            try
            {
                var cupon = await _cuponService.GetCuponByCodigoAsync(codigo);
                if (!cupon.Activo || cupon.FechaFin < DateTime.UtcNow)
                    return BadRequest(new { exito = false, mensaje = "El cupón ha expirado o no es válido" });

                return Ok(new { exito = true, data = cupon });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { exito = false, mensaje = "Cupón no encontrado" });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCupon(CuponCreateDto dto)
        {
            var cupon = await _cuponService.CreateCuponAsync(dto);
            return CreatedAtAction(nameof(GetCuponById), new { id = cupon.Id }, new { exito = true, mensaje = "Cupón creado exitosamente", data = cupon });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCupon(int id, CuponUpdateDto dto)
        {
            var cupon = await _cuponService.UpdateCuponAsync(id, dto);
            return Ok(new { exito = true, mensaje = "Cupón actualizado exitosamente", data = cupon });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupon(int id)
        {
            await _cuponService.DeleteCuponAsync(id);
            return Ok(new { exito = true, mensaje = "Cupón desactivado exitosamente", data = (object)null });
        }
    }
}
