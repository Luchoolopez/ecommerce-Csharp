using EcommerceStore.DTOs.CategoriaDto;
using EcommerceStore.Services.CategoriaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias([FromQuery] int page = 1, [FromQuery] int limit = 100)
        {
            var categorias = await _categoriaService.GetCategorias(page, limit);
            return Ok(new { exito = true, mensaje = "Categorias obtenidas exitosamente", data = categorias });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _categoriaService.GetCategoriaById(id);
            return Ok(new { exito = true, mensaje = "Categoria obtenida exitosamente", data = categoria });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoria(CategoriaCreateDto dto)
        {
            var nuevaCategoria = await _categoriaService.CreateCategoria(dto);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = nuevaCategoria.Id }, new { exito = true, mensaje = "Categoria creada exitosamente", data = nuevaCategoria });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaUpdateDto dto)
        {
            var categoriaActualizada = await _categoriaService.UpdateCategoria(id, dto);
            return Ok(new { exito = true, mensaje = "Categoria actualizada exitosamente", data = categoriaActualizada });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            await _categoriaService.DeleteCategoria(id);
            return Ok(new { exito = true, mensaje = "Categoria eliminada exitosamente", data = (object)null });
        }
    }
}