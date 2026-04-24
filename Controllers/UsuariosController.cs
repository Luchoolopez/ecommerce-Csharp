using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Services.UsuarioService;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{userId}")] 
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _usuarioService.GetUser(userId);
            return Ok(new { exito = true, mensaje = "Usuario encontrado", data = user });
        }

        [HttpGet] 
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usuarioService.GetUsers();
            return Ok(new { exito = true, mensaje = "Usuarios obtenidos exitosamente", data = users });
        }

        [HttpPut("{userId}")] 
        public async Task<IActionResult> UpdateUser(int userId, UsuarioUpdateDto usuarioDto)
        {
            var updatedUser = await _usuarioService.UpdateUser(userId, usuarioDto);
            return Ok(new { exito = true, mensaje = "Usuario actualizado exitosamente", data = updatedUser });
        }

        [HttpPut("{userId}/change-password")] 
        public async Task<IActionResult> ChangePassword(int userId, string newPassword)
        {
            // Hacer a futuro
            return BadRequest(new { exito = false, mensaje = "Ruta no implementada aún", data = (object)null });
        }

        [HttpDelete("{userId}")] 
        public async Task<IActionResult> DeleteUser(int userId)
        {
            await _usuarioService.DeleteUser(userId);
            return Ok(new { exito = true, mensaje = "Usuario eliminado exitosamente", data = (object)null });
        }
    }
}