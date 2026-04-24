using Microsoft.AspNetCore.Mvc;
using ProductSystem.BLL.Interfaces;

namespace ProductSystem.API.Controllers;

[Route("api/image")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IFileService _fileService;

    public ImageController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var filePath = await _fileService.UploadImageAsync(file);
            return Ok(new { FilePath = filePath });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
