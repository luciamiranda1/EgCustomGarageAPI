using Application.Models.Request;         
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService) => _orderService = orderService;

        [HttpPost("create")]
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderForAddRequest request)
        {
            try
            {
                var sub = User.FindFirst("sub")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(sub, out var clienteId)) return Unauthorized();

                var order = await _orderService.CreateOrderAsync(clienteId, request.ProductId, request.Quantity);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("clients/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetClientsByProductId(int productId)
        {
            try
            {
                var clients = await _orderService.GetClientsByProductIdAsync(productId);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
