using EcommerceStore.DTOs.AuthDto;
using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                if (ex.Message == "El email ya esta registrado")
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

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { mensaje = "Usuario no autenticado" });
                }
                var profile = await _authService.GetMyProfile(userId);
                return Ok(new
                {
                    mensaje = "Perfil obtenido exitosamente",
                    data = profile
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrio un error al obtener el perfil.", error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Unauthorized(new { mensaje = "Usuario no autenticado" });
                }
                await _authService.Logout(userId);
                return Ok(new { mensaje = "Cierre de sesion exitoso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrio un error al cerrar sesion.", error = ex.Message });
            }
        }
    }
}
