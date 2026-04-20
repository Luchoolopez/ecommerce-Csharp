using EcommerceStore.Data;
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
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerTodosAsync();
                if (usuarios == null)
                {
                    return StatusCode(200, new { mensaje = "No se encontraron usuarios" });
                }
                return Ok(usuarios);
            }catch(Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrio un error al obtener los usuarios" });
            }
        }
    }
}
