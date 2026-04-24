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
            var usuarioCreado = await _authService.RegisterAsync(dto);
            return StatusCode(201, new { exito = true, mensaje = "Usuario registrado exitosamente", data = usuarioCreado });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginDto dto)
        {
            var authResponse = await _authService.LoginAsync(dto);
            return Ok(new { exito = true, mensaje = "Inicio de sesion exitoso", data = authResponse });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            return Ok(new { exito = true, mensaje = "Token actualizado exitosamente", data = result });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { exito = false, mensaje = "Usuario no autenticado", data = (object)null });

            var profile = await _authService.GetMyProfile(userId);
            return Ok(new { exito = true, mensaje = "Perfil obtenido exitosamente", data = profile });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized(new { exito = false, mensaje = "Usuario no autenticado", data = (object)null });

            await _authService.Logout(userId);
            return Ok(new { exito = true, mensaje = "Cierre de sesion exitoso", data = (object)null });
        }
    }
}    