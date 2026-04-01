using BCrypt.Net;
using EcommerceStore.Data;
using EcommerceStore.DTOs;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services

{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioResponseDto> RegisterAsync(UsuarioRegisterDto dto)
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

            return new UsuarioResponseDto
            {
                Id = nuevoUsuario.Id,
                Nombre = nuevoUsuario.Nombre,
                Email = nuevoUsuario.Email,
                Rol = nuevoUsuario.Rol.ToString(), //convierte el enum a texto
                Telefono = nuevoUsuario.Telefono,
                Activo = nuevoUsuario.Activo
            };
        }
    }
}
