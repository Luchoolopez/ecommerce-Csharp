using EcommerceStore.Data;
using EcommerceStore.DTOs.DireccionDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.DireccionService
{
    public class DireccionService : IDireccionService
    {
        private readonly AppDbContext _context;

        public DireccionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DireccionResponseDto>> GetDireccionesUsuario(int usuarioId)
        {
            return await _context.Direcciones
                .Where(d => d.UsuarioId == usuarioId)
                .Select(d => new DireccionResponseDto
                {
                    Id = d.Id,
                    Calle = d.Calle,
                    Numero = d.Numero,
                    Piso = d.Piso,
                    Dpto = d.Dpto,
                    Ciudad = d.Ciudad,
                    Provincia = d.Provincia,
                    CodigoPostal = d.CodigoPostal,
                    Pais = d.Pais,
                    EsPrincipal = d.EsPrincipal
                })
                .ToListAsync();
        }

        public async Task<DireccionResponseDto> GetDireccionById(int usuarioId, int direccionId)
        {
            var direccion = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId && d.Id == direccionId);

            if (direccion == null)
            {
                throw new KeyNotFoundException("Dirección no encontrada o no pertenece al usuario.");
            }

            return new DireccionResponseDto
            {
                Id = direccion.Id,
                Calle = direccion.Calle,
                Numero = direccion.Numero,
                Piso = direccion.Piso,
                Dpto = direccion.Dpto,
                Ciudad = direccion.Ciudad,
                Provincia = direccion.Provincia,
                CodigoPostal = direccion.CodigoPostal,
                Pais = direccion.Pais,
                EsPrincipal = direccion.EsPrincipal
            };
        }

        public async Task<DireccionResponseDto> CreateDireccion(int usuarioId, DireccionCreateDto dto)
        {
            var hasDirecciones = await _context.Direcciones.AnyAsync(d => d.UsuarioId == usuarioId);
            bool esPrincipal = dto.EsPrincipal || !hasDirecciones;

            if (esPrincipal && hasDirecciones)
            {
                var direccionesPrincipales = await _context.Direcciones
                    .Where(d => d.UsuarioId == usuarioId && d.EsPrincipal)
                    .ToListAsync();

                foreach (var dir in direccionesPrincipales)
                {
                    dir.EsPrincipal = false;
                }
            }

            var nuevaDireccion = new Direccion
            {
                UsuarioId = usuarioId,
                Calle = dto.Calle,
                Numero = dto.Numero,
                Piso = dto.Piso,
                Dpto = dto.Dpto,
                Ciudad = dto.Ciudad,
                Provincia = dto.Provincia,
                CodigoPostal = dto.CodigoPostal,
                Pais = string.IsNullOrWhiteSpace(dto.Pais) ? "Argentina" : dto.Pais,
                EsPrincipal = esPrincipal
            };

            _context.Direcciones.Add(nuevaDireccion);
            await _context.SaveChangesAsync();

            return new DireccionResponseDto
            {
                Id = nuevaDireccion.Id,
                Calle = nuevaDireccion.Calle,
                Numero = nuevaDireccion.Numero,
                Piso = nuevaDireccion.Piso,
                Dpto = nuevaDireccion.Dpto,
                Ciudad = nuevaDireccion.Ciudad,
                Provincia = nuevaDireccion.Provincia,
                CodigoPostal = nuevaDireccion.CodigoPostal,
                Pais = nuevaDireccion.Pais,
                EsPrincipal = nuevaDireccion.EsPrincipal
            };
        }

        public async Task<DireccionResponseDto> UpdateDireccion(int usuarioId, int direccionId, DireccionUpdateDto dto)
        {
            var direccionToUpdate = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId && d.Id == direccionId);

            if (direccionToUpdate == null)
            {
                throw new KeyNotFoundException("Dirección no encontrada o no pertenece al usuario.");
            }

            if (!string.IsNullOrEmpty(dto.Calle)) direccionToUpdate.Calle = dto.Calle;
            if (dto.Numero != null) direccionToUpdate.Numero = dto.Numero;
            if (dto.Piso != null) direccionToUpdate.Piso = dto.Piso;
            if (dto.Dpto != null) direccionToUpdate.Dpto = dto.Dpto;
            if (!string.IsNullOrEmpty(dto.Ciudad)) direccionToUpdate.Ciudad = dto.Ciudad;
            if (!string.IsNullOrEmpty(dto.Provincia)) direccionToUpdate.Provincia = dto.Provincia;
            if (!string.IsNullOrEmpty(dto.CodigoPostal)) direccionToUpdate.CodigoPostal = dto.CodigoPostal;
            if (!string.IsNullOrEmpty(dto.Pais)) direccionToUpdate.Pais = dto.Pais;

            if (dto.EsPrincipal.HasValue && dto.EsPrincipal.Value && !direccionToUpdate.EsPrincipal)
            {
                var direccionesPrincipales = await _context.Direcciones
                    .Where(d => d.UsuarioId == usuarioId && d.EsPrincipal && d.Id != direccionId)
                    .ToListAsync();

                foreach (var dir in direccionesPrincipales)
                {
                    dir.EsPrincipal = false;
                }
                
                direccionToUpdate.EsPrincipal = true;
            }
            else if (dto.EsPrincipal.HasValue && !dto.EsPrincipal.Value && direccionToUpdate.EsPrincipal)
            {
                direccionToUpdate.EsPrincipal = false;
            }

            await _context.SaveChangesAsync();

            return new DireccionResponseDto
            {
                Id = direccionToUpdate.Id,
                Calle = direccionToUpdate.Calle,
                Numero = direccionToUpdate.Numero,
                Piso = direccionToUpdate.Piso,
                Dpto = direccionToUpdate.Dpto,
                Ciudad = direccionToUpdate.Ciudad,
                Provincia = direccionToUpdate.Provincia,
                CodigoPostal = direccionToUpdate.CodigoPostal,
                Pais = direccionToUpdate.Pais,
                EsPrincipal = direccionToUpdate.EsPrincipal
            };
        }

        public async Task<bool> DeleteDireccion(int usuarioId, int direccionId)
        {
            var direccionToDelete = await _context.Direcciones
                .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId && d.Id == direccionId);

            if (direccionToDelete == null)
            {
                throw new KeyNotFoundException("Dirección no encontrada o no pertenece al usuario.");
            }

            _context.Direcciones.Remove(direccionToDelete);
            await _context.SaveChangesAsync();

            if (direccionToDelete.EsPrincipal)
            {
                var otraDireccion = await _context.Direcciones
                    .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId);
                    
                if (otraDireccion != null)
                {
                    otraDireccion.EsPrincipal = true;
                    await _context.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}
