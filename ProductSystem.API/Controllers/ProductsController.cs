using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Interfaces;

namespace ProductSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IFileService _fileService;

        public ProductsController(IProductManager productManager, IFileService fileService)
        {
            _productManager = productManager;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductReadDto>>> GetAll([FromQuery] ProductQueryDto query)
        {
            var products = await _productManager.GetProductsAsync(query);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductReadDto>> GetById(int id)
        {
            var product = await _productManager.GetByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<ProductReadDto>> Create([FromBody] ProductCreateDto dto)
        {
            var result = await _productManager.AddAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Update(int id, [FromQuery] ProductUpdateDto dto)
        {
            var result = await _productManager.UpdateAsync(id, dto);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productManager.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            return NoContent();
        }
    }
}
