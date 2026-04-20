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
        public async Task<IActionResult> GetCategorias()
        {
            try
            {
                var categorias = await _categoriaService.GetCategorias();
                if(categorias == null)
                {
                    return NotFound(new
                    {
                        exito = false,
                        mensaje = "Cateogira no encontrada"
                    });
                }
                return Ok(new
                {
                    exito = true,
                    mensaje = "Categorias obtenidas exitosamente",
                    data = categorias
                });
            } catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al obtener las categorias",
                    error = ex.Message,
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            try
            {
               var categoria = await _categoriaService.GetCategoriaById(id);
                return Ok(new {
                    exito = true,
                    mensaje = "Categoria obtenida exitosamente",
                    data = categoria
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al obtener la categoria",
                    error = ex.Message,
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategoria(CategoriaCreateDto dto)
        {
            try
            {
                var nuevaCategoria = await _categoriaService.CreateCategoria(dto);
                return CreatedAtAction(nameof(GetCategoriaById), new { id = nuevaCategoria.Id }, nuevaCategoria);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al crear la categoria",
                    error = ex.Message,
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaUpdateDto dto)
        {
            try
            {
                var categoriaActualizada = await _categoriaService.UpdateCategoria(id, dto);
                return Ok(categoriaActualizada);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al actualizar la categoria",
                    error = ex.Message,
                });
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            try
            {
                await _categoriaService.DeleteCategoria(id);
                return Ok(new
                {
                    exito = true,
                    mensaje = "Categoria eliminada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    exito = false,
                    mensaje = "Error al eliminar la categoria",
                    error = ex.Message,
                });
            }
        }
    }
}
