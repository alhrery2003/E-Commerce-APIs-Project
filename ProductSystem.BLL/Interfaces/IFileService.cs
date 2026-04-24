using Microsoft.AspNetCore.Http;

namespace ProductSystem.BLL.Interfaces;

public interface IFileService
{
    Task<string> UploadImageAsync(IFormFile file);
    Task<string> UpdateImageAsync(IFormFile file, string oldImageUrl);
    void DeleteImage(string imageUrl);
}
