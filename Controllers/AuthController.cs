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
                return StatusCode(201, new
                {
                    mensaje = "Usuario registrado exitosamente",
                    usuario = usuarioCreado
                });

            }
            catch (Exception ex)
            {
                if (ex.Message == "El email ya se encuentra registrado")
                {
                    return BadRequest(new { mensaje = ex.Message });
                }

                return StatusCode(500, new { mensaje = "Ocurrio un error al registrar el usuario.", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginDto dto)
        {
            try
            {
                var authResponse = await _authService.LoginAsync(dto);
                return StatusCode(200, new
                {
                    mensaje = "Inicio de sesion exitoso",
                    data = authResponse
                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Credenciales invalidas")
                {
                    return Unauthorized(new { mensaje = ex.Message });
                }
                return StatusCode(500, new { mensaje = "Ocurrio un error al iniciar sesion.", error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            try
            {
               var result = await _authService.RefreshTokenAsync(dto);
                return StatusCode(200, new
                {
                    mensaje = "Token actualizado exitosamente",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }
    }
}
