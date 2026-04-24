using Microsoft.AspNetCore.Mvc;
using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Interfaces;

namespace ProductSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authManager.RegisterAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { Message = result.ErrorMessage });

            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authManager.LoginAsync(dto);
            if (!result.IsSuccess)
                return Unauthorized(new { Message = result.ErrorMessage });

            return Ok(result.Data);
        }
    }
}
