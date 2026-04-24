using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Interfaces;
using System.Security.Claims;

namespace ProductSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _cartManager.GetUserCartAsync(userId);
            if (!result.IsSuccess) return NotFound(new { message = result.ErrorMessage });
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _cartManager.AddToCartAsync(userId, dto);
            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage });
            return Ok(new { message = "Item added to cart" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCartItem([FromQuery] UpdateCartItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _cartManager.UpdateCartItemQuantityAsync(userId, dto);
            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage });
            return Ok(new { message = "Cart updated" });
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _cartManager.RemoveFromCartAsync(userId, productId);
            if (!result.IsSuccess) return BadRequest(new { message = result.ErrorMessage });
            return Ok(new { message = "Item removed" });
        }
    }
}
