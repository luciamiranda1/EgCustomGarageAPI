using Application.Models.Requests;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
