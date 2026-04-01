using EcommerceStore.DTOs;
using EcommerceStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
    

    [HttpPost("register")]
        public async Task<IActionResult> Register(UsuarioRegisterDto dto)
        {
            try
            {
                var usuarioCreado = await _authService.RegisterAsync(dto);
                return StatusCode(201, new { 
                    mensaje = "Usuario registrado exitosamente",
                    usuario = usuarioCreado
                });

            }
            catch (Exception ex)
            {
                if(ex.Message == "El email ya se encuentra registrado")
                {
                    return BadRequest(new { mensaje = ex.Message });
                }

                return StatusCode(500, new { mensaje = "Ocurrio un error al registrar el usuario.", error = ex.Message });
            }
        }
    }
}
