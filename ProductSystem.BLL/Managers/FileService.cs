using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductSystem.BLL.Interfaces;

namespace ProductSystem.BLL.Managers;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
    private static readonly string[] PermittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        ValidateFile(file);

        var webRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
        var uploadsFolder = Path.Combine(webRootPath, "images");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/images/{uniqueFileName}";
    }

    public async Task<string> UpdateImageAsync(IFormFile file, string oldImageUrl)
    {
        if (!string.IsNullOrEmpty(oldImageUrl))
        {
            DeleteImage(oldImageUrl);
        }

        return await UploadImageAsync(file);
    }

    public void DeleteImage(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        var webRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
        var imagePath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty.", nameof(file));
        }

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException($"File size exceeds the limit of {MaxFileSize / 1024 / 1024} MB.", nameof(file));
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !PermittedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.", nameof(file));
        }
    }
}
