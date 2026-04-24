using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSystem.BLL.Interfaces;
using System.Security.Claims;

namespace ProductSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _orderManager.PlaceOrderAsync(userId);
            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage });

            return CreatedAtAction(nameof(GetOrderDetails), new { id = result.Data!.Id }, result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _orderManager.GetUserOrdersAsync(userId);
            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _orderManager.GetOrderDetailsAsync(userId, id);
            if (!result.IsSuccess) return NotFound(new { message = result.ErrorMessage });

            return Ok(result.Data);
        }
    }
}
