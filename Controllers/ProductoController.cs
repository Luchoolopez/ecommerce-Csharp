using EcommerceStore.DTOs.ProductoDto;
using EcommerceStore.Services.ProductoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos([FromQuery] ProductoFilterDto filters)
        {
            var productos = await _productoService.GetProductos(filters);
            return Ok(new { exito = true, mensaje = "Productos obtenidos exitosamente", data = productos });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoById(id);
            return Ok(new { exito = true, mensaje = "Producto obtenido exitosamente", data = producto });
        }

        [HttpGet("featured")]
        public async Task<IActionResult> GetFeaturedProductos([FromQuery] int limit = 10)
        {
            var productos = await _productoService.GetFeaturedProductos(limit);
            return Ok(new { exito = true, mensaje = "Productos destacados obtenidos exitosamente", data = productos });
        }

        [HttpGet("new")]
        public async Task<IActionResult> GetNewProductos([FromQuery] int limit = 10)
        {
            var productos = await _productoService.GetNewProductos(limit);
            return Ok(new { exito = true, mensaje = "Productos nuevos obtenidos exitosamente", data = productos });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProducto(ProductoCreateDto dto)
        {
            var nuevoProducto = await _productoService.CreateProducto(dto);
            return CreatedAtAction(nameof(GetProductoById), new { id = nuevoProducto.Id }, new { exito = true, mensaje = "Producto creado exitosamente", data = nuevoProducto });
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, ProductoUpdateDto dto)
        {
            var productoActualizado = await _productoService.UpdateProducto(id, dto);
            return Ok(new { exito = true, mensaje = "Producto actualizado exitosamente", data = productoActualizado });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            await _productoService.DeleteProducto(id);
            return Ok(new { exito = true, mensaje = "Producto eliminado exitosamente", data = (object)null });
        }
    }
}
