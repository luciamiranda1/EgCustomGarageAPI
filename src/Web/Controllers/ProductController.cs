using Application.Models.Response;
using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, Cliente")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] bool includeDeleted = false)
        {
            var products = await _service.GetAllAsync(includeDeleted);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Admin, Cliente")]
        public async Task<ActionResult<ProductDto>> GetById(int id, [FromQuery] bool includeDeleted = false)
        {
            var product = await _service.GetByIdAsync(id, includeDeleted);
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest req)
        {
            var created = await _service.CreateAsync(req);
            if (created == null) return BadRequest("No se pudo crear el producto.");

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
       //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest req)
        {
            var ok = await _service.UpdateAsync(id, req);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
