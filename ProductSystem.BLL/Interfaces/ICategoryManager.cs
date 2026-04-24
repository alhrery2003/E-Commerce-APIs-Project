using ProductSystem.BLL.DTOs;

namespace ProductSystem.BLL.Interfaces
{
    public interface ICategoryManager
    {
        Task<IEnumerable<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto?> GetByIdAsync(int id);
        Task<CategoryReadDto> AddAsync(CategoryCreateDto category);
        Task<bool> UpdateAsync(int id, CategoryUpdateDto category);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateImageAsync(int id, string imageUrl);
    }
}
