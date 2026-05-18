using EcommerceStore.Data;
using EcommerceStore.DTOs.CuponDto;
using EcommerceStore.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStore.Services.CuponService
{
    public class CuponService : ICuponService
    {
        private readonly AppDbContext _context;

        public CuponService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CuponResponseDto>> GetCuponesAsync(bool onlyActive = false)
        {
            var query = _context.Cupones.AsNoTracking().AsQueryable();

            if (onlyActive)
            {
                query = query.Where(c => c.Activo && c.FechaFin >= DateTime.UtcNow);
            }

            var cupones = await query.OrderByDescending(c => c.FechaCreacion).ToListAsync();
            return cupones.Select(MapToDto);
        }

        public async Task<CuponResponseDto> GetCuponByIdAsync(int id)
        {
            var cupon = await _context.Cupones.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (cupon == null) throw new KeyNotFoundException($"Cupón con id {id} no encontrado");
            return MapToDto(cupon);
        }

        public async Task<CuponResponseDto> GetCuponByCodigoAsync(string codigo)
        {
            var cupon = await _context.Cupones.AsNoTracking().FirstOrDefaultAsync(c => c.Codigo.ToLower() == codigo.ToLower());
            if (cupon == null) throw new KeyNotFoundException($"Cupón con código {codigo} no encontrado");
            return MapToDto(cupon);
        }

        public async Task<CuponResponseDto> CreateCuponAsync(CuponCreateDto dto)
        {
            var existe = await _context.Cupones.AnyAsync(c => c.Codigo.ToLower() == dto.Codigo.ToLower());
            if (existe) throw new Exception("Ya existe un cupón con este código.");

            var cupon = new Cupon
            {
                Codigo = dto.Codigo.ToUpper(),
                Descripcion = dto.Descripcion,
                Tipo = dto.Tipo,
                Valor = dto.Valor,
                MontoMinimo = dto.MontoMinimo,
                UsosMaximos = dto.UsosMaximos,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Activo = dto.Activo
            };

            _context.Cupones.Add(cupon);
            await _context.SaveChangesAsync();

            return MapToDto(cupon);
        }

        public async Task<CuponResponseDto> UpdateCuponAsync(int id, CuponUpdateDto dto)
        {
            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Id == id);
            if (cupon == null) throw new KeyNotFoundException($"Cupón con id {id} no encontrado");

            if (!string.IsNullOrEmpty(dto.Codigo) && dto.Codigo.ToUpper() != cupon.Codigo)
            {
                var existe = await _context.Cupones.AnyAsync(c => c.Codigo.ToLower() == dto.Codigo.ToLower() && c.Id != id);
                if (existe) throw new Exception("Ya existe otro cupón con este código.");
                cupon.Codigo = dto.Codigo.ToUpper();
            }

            if (dto.Descripcion != null) cupon.Descripcion = dto.Descripcion;
            if (dto.Tipo.HasValue) cupon.Tipo = dto.Tipo.Value;
            if (dto.Valor.HasValue) cupon.Valor = dto.Valor.Value;
            if (dto.MontoMinimo.HasValue) cupon.MontoMinimo = dto.MontoMinimo.Value;
            if (dto.UsosMaximos.HasValue) cupon.UsosMaximos = dto.UsosMaximos.Value;
            if (dto.FechaInicio.HasValue) cupon.FechaInicio = dto.FechaInicio.Value;
            if (dto.FechaFin.HasValue) cupon.FechaFin = dto.FechaFin.Value;
            if (dto.Activo.HasValue) cupon.Activo = dto.Activo.Value;

            await _context.SaveChangesAsync();
            return MapToDto(cupon);
        }

        public async Task<bool> DeleteCuponAsync(int id)
        {
            var cupon = await _context.Cupones.FirstOrDefaultAsync(c => c.Id == id);
            if (cupon == null) throw new KeyNotFoundException($"Cupón con id {id} no encontrado");

            cupon.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private CuponResponseDto MapToDto(Cupon c)
        {
            return new CuponResponseDto
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Descripcion = c.Descripcion,
                Tipo = c.Tipo.ToString(),
                Valor = c.Valor,
                MontoMinimo = c.MontoMinimo,
                UsosMaximos = c.UsosMaximos,
                UsosActuales = c.UsosActuales,
                FechaInicio = c.FechaInicio,
                FechaFin = c.FechaFin,
                Activo = c.Activo,
                FechaCreacion = c.FechaCreacion
            };
        }
    }
}
