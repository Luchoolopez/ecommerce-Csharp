using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Services.UsuarioService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace EcommerceStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuarioService;
        public UsuariosController(IUsuariosService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet("{userId}")] 
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _usuarioService.GetUser(userId);
            return Ok(new { exito = true, mensaje = "Usuario encontrado", data = user });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet] 
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 20)
        {
            var users = await _usuarioService.GetUsers(page, limit);
            return Ok(new { exito = true, mensaje = "Usuarios obtenidos exitosamente", data = users });
        }

        [Authorize]
        [HttpPut("{userId}")] 
        public async Task<IActionResult> UpdateUser(int userId, UsuarioUpdateDto usuarioDto)
        {
            var tokenId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if(tokenId != userId)
            {
                return Forbid();
            }

            var updatedUser = await _usuarioService.UpdateUser(userId, usuarioDto);
            return Ok(new { exito = true, mensaje = "Usuario actualizado exitosamente", data = updatedUser });
        }

        [HttpPut("{userId}/change-password")] 
        public async Task<IActionResult> ChangePassword(int userId, string newPassword)
        {
            // Hacer a futuro
            return BadRequest(new { exito = false, mensaje = "Ruta no implementada aún", data = (object)null });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")] 
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _usuarioService.DeleteUser(userId);
            return Ok(new { exito = true, mensaje = "Usuario eliminado exitosamente", data = (object)null });
        }
    }
}