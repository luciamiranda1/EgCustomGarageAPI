using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string name)
        {
            var user = await _service.GetAsync(name);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UserForUpdateRequest body)
        {
            var exists = await _service.GetByIdAsync(id, includeDeleted: true);
            if (exists == null) return NotFound();

            await _service.UpdateAsync(id, body);
            return Ok("Usuario actualizado correctamente.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromQuery] bool includeDeleted = false)
        {
            var users = await _service.GetAsync(includeDeleted);
            return Ok(users);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] UserForAddRequest body)
        {
            var id = await _service.AddUserAsync(body);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("create-admin-first-time")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateFirstAdmin()
        {
            var existingAdmin = (await _service.GetAsync())
                .FirstOrDefault(u => u.Role == UserRole.Admin);

            if (existingAdmin != null)
                return BadRequest("Ya existe un administrador.");

            var req = new UserForAddRequest
            {
                Name = "Admin",
                Email = "admin@demo.com",
                Password = "1234"
            };

            var id = await _service.AddUserAsync(req);
            await _service.UpdateRoleAsync(id, UserRole.Admin);

            return Ok("Admin creado correctamente.");
        }
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(int id, [FromBody] string newRole)
        {
            if (!Enum.TryParse<UserRole>(newRole, true, out var parsedRole))
                return BadRequest("Rol inválido. Usa: Admin o Client");

            await _service.UpdateRoleAsync(id, parsedRole);
            return Ok("Rol actualizado.");
        }

    }
}
