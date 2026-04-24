using ProductSystem.BLL.Interfaces;
using ProductSystem.BLL.DTOs;
using ProductSystem.DAL.Entities;
using ProductSystem.DAL.UnitOfWork;

namespace ProductSystem.BLL.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public CategoryManager(IUnitOfWork uow, IFileService fileService)
        {
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<CategoryReadDto> AddAsync(CategoryCreateDto category)
        {
            var entity = new Category
            {
                Name = category.Name,
                ImagePath = category.ImageUrl
            };

            await _uow.Categories.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return MapToReadDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.Categories.GetByIdAsync(id);
            if (entity == null) return false;

            _fileService.DeleteImage(entity.ImagePath);
            _uow.Categories.Delete(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllAsync()
        {
            var categories = await _uow.Categories.GetAllAsync();
            return categories.Select(MapToReadDto);
        }

        public async Task<CategoryReadDto?> GetByIdAsync(int id)
        {
            var entity = await _uow.Categories.GetByIdAsync(id);
            return entity is null ? null : MapToReadDto(entity);
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto category)
        {
            var entity = await _uow.Categories.GetByIdAsync(id);
            if (entity is null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(category.ImageUrl) && entity.ImagePath != category.ImageUrl)
            {
                if (!string.IsNullOrEmpty(entity.ImagePath))
                {
                    _fileService.DeleteImage(entity.ImagePath);
                }
                entity.ImagePath = category.ImageUrl;
            }

            if (category.Name != null)
                entity.Name = category.Name;

            _uow.Categories.Update(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateImageAsync(int id, string imageUrl)
        {
            var entity = await _uow.Categories.GetByIdAsync(id);
            if (entity is null) return false;

            _fileService.DeleteImage(entity.ImagePath);
            entity.ImagePath = imageUrl;
            await _uow.SaveChangesAsync();
            return true;
        }

        private static CategoryReadDto MapToReadDto(Category category)
        {
            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImagePath
            };
        }
    }
}
