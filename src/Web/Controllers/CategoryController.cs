using Application.Models.Requests;
using Application.Models;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[Authorize(Roles = "Admin, Client")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] bool includeDeleted = false)
        {
            var categories = await _categoryService.GetAllAsync(includeDeleted);

            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,     // mapeo desde entidad
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = c.Name
                }).ToList()
            });

            return Ok(categoryDtos);
        }

        //[Authorize(Roles = "Admin, Client")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id, [FromQuery] bool includeDeleted = false)
        {
            var category = await _categoryService.GetByIdAsync(id, includeDeleted);
            if (category == null) return NotFound();

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Products = category.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = category.Name
                }).ToList()
            };

            return Ok(categoryDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest createDto)
        {
            var category = new Category { Name = createDto.Name };

            var created = await _categoryService.CreateAsync(category);

            var categoryDto = new CategoryDto
            {
                Id = created.Id,
                Name = created.Name,
                Products = new List<ProductDto>()
            };

            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryRequest updateDto)
        {
            var category = await _categoryService.GetByIdAsync(id, includeDeleted: true);
            if (category == null) return NotFound();

            category.Name = updateDto.Name;

            var updated = await _categoryService.UpdateAsync(category);
            if (!updated) return BadRequest();

            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
