using BCrypt.Net;
using EcommerceStore.Data;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using EcommerceStore.DTOs.UsuarioDto;
using EcommerceStore.DTOs.AuthDto;


namespace EcommerceStore.Services.AuthService

{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config; //es como usar process.env

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenerateAccesToken(Usuario usuario)
        {
            var jwtKey = _config["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //creamos el paylod
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), //id del usuario
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("rol", usuario.Rol.ToString())
            };

            //crea el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<AuthResponseDto> RegisterAsync(UsuarioRegisterDto dto)
        {
            var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existeUsuario)
            {
                throw new Exception("El email ya esta registrado");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Password = hashedPassword,
            };

            //lo guarda en la db
            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            var usuarioResponse = new UsuarioResponseDto
            {
                Id = nuevoUsuario.Id,
                Nombre = nuevoUsuario.Nombre,
                Email = nuevoUsuario.Email,
                Rol = nuevoUsuario.Rol.ToString(),
                Telefono = nuevoUsuario.Telefono,
                Activo = nuevoUsuario.Activo
            };

            var accessToken = GenerateAccesToken(nuevoUsuario);
            var refreshToken = GenerateRefreshToken();

            return new AuthResponseDto
            {
                Usuario = usuarioResponse,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public async Task<AuthResponseDto> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
            {
                throw new Exception("Credenciales invalidas");
            }
            if (!usuario.Activo)
            {
                throw new Exception("Cuenta inactiva");
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Credenciales invalidas");
            }

            var accessToken = GenerateAccesToken(usuario);
            var refreshToken = GenerateRefreshToken();

            usuario.FechaUltimoAcceso = DateTime.UtcNow;
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            var usuarioResponse = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo
            };

            return new AuthResponseDto
            {
                Usuario = usuarioResponse,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);
            if(usuario == null)
            {
                throw new Exception("Refresh token invalido");
            }
            if(usuario.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new Exception("Refresh token expirado");
            }
            var accessToken = GenerateAccesToken(usuario);
            var refreshToken = GenerateRefreshToken();
         
            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();


            var usuarioResponse = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Telefono = usuario.Telefono,
                Activo = usuario.Activo
            };

            return new AuthResponseDto
            {
                Usuario = usuarioResponse,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<UsuarioResponseDto> GetMyProfile(string userId)
        {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado");
                }
    
                var usuarioResponse = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Rol = usuario.Rol.ToString(),
                    Telefono = usuario.Telefono,
                    Activo = usuario.Activo
                };
    
                return usuarioResponse;
        }

        public async Task Logout(string userId)
        {
            int idNumerico = int.Parse(userId);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == idNumerico);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");
            }
            usuario.RefreshToken = null;
            usuario.RefreshTokenExpires = null;
            await _context.SaveChangesAsync();
        }
    }
}
