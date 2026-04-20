using EcommerceStore.Data;
using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Services.UsuarioService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Controllers
{
    [ApiController] //habilita validaciones automaticas y comportamientos tipicos del api rest
    [Route("api/[controller]")] //route define la ruta base, [controller] es un placeholder que se reemplaza por el nombre del controlador sin la palabra "Controller", en este caso "usuarios"
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuarioService;
        public UsuariosController(IUsuariosService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _usuarioService.GetUser(userId);
                return Ok(new
                {
                    exito = true,
                    mensaje = "Usuario encontrado",
                    data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al encontrar al usuario",
                    error = ex.Message,
                });
            }
        }

        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _usuarioService.GetUsers();
                return Ok(new
                {
                    exito = true,
                    mensaje = "Usuarios obtenidos exitosamente",
                    data = users
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al obtener los usuarios",
                    error = ex.Message,
                });
            }
        }

        public async Task<IActionResult> UpdateUser(int userId, UsuarioUpdateDto usuarioDto)
        {
            try
            {
                var updatedUser = await _usuarioService.UpdateUser(userId, usuarioDto);
                return Ok(new
                {
                    exito = true,
                    mensaje = "Usuario actualizado exitosamente",
                    data = updatedUser
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al actualizar el usuario",
                    error = ex.Message,
                });
            }
        }

        public async Task<IActionResult> ChangePassword(int userId, string newPassword)
        {
            //hacer a futuro
            return BadRequest();
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var deleteUser = await _usuarioService.DeleteUser(userId);
                return Ok(new
                {
                    exito = deleteUser,
                    mensaje = deleteUser ? "Usuario eliminado exitosamente" : "No se pudo eliminar el usuario",
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al eliminar el usuario",
                    error = ex.Message,
                });
            }
        }
}
