using ProductSystem.BLL.Interfaces;
using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Shared;
using ProductSystem.DAL.Entities;
using ProductSystem.DAL.UnitOfWork;

namespace ProductSystem.BLL.Managers
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IFileService _fileService;

        public ProductManager(IUnitOfWork uow, IFileService fileService)
        {
            _uow = uow;
            _fileService = fileService;
        }

        public async Task<Result<ProductReadDto>> AddAsync(ProductCreateDto product)
        {
            var category = await _uow.Categories.GetByIdAsync(product.CategoryId);
            if (category is null)
            {
                return Result<ProductReadDto>.Failure("Invalid category id.");
            }

            var entity = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };

            await _uow.Products.AddAsync(entity);
            await _uow.SaveChangesAsync();

            var dto = MapToReadDto(entity);
            dto.CategoryName = category.Name;

            return Result<ProductReadDto>.Success(dto);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var entity = await _uow.Products.GetByIdAsync(id);
            if (entity == null) return Result.Failure("Product not found");

            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                _fileService.DeleteImage(entity.ImageUrl);
            }
            _uow.Products.Delete(entity);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<PaginatedResult<ProductReadDto>> GetProductsAsync(ProductQueryDto query)
        {
            var allProducts = await _uow.Products.GetProductsWithCategoryAsync();

            // Filter
            var filtered = allProducts.AsQueryable();
            if (query.CategoryId.HasValue)
                filtered = filtered.Where(p => p.CategoryId == query.CategoryId.Value);
            if (!string.IsNullOrEmpty(query.Name))
                filtered = filtered.Where(p => p.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));

            // Paginate
            var totalCount = filtered.Count();
            var paged = filtered.Skip((query.PageNumber - 1) * query.PageSize)
                                .Take(query.PageSize)
                                .Select(MapToReadDto)
                                .ToList();

            return new PaginatedResult<ProductReadDto>
            {
                Items = paged,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<ProductReadDto?> GetByIdAsync(int id)
        {
            var entity = await _uow.Products.GetByIdWithCategoryAsync(id);
            return entity is null ? null : MapToReadDto(entity);
        }

        public async Task<Result> UpdateAsync(int id, ProductUpdateDto product)
        {
            var entity = await _uow.Products.GetByIdAsync(id);
            if (entity is null)
            {
                return Result.Failure("Product not found");
            }

            if (product.CategoryId.HasValue)
            {
                var category = await _uow.Categories.GetByIdAsync(product.CategoryId.Value);
                if (category is null)
                {
                    return Result.Failure("Invalid category id.");
                }
                entity.CategoryId = product.CategoryId.Value;
            }

            if (!string.IsNullOrEmpty(product.ImageUrl) && entity.ImageUrl != product.ImageUrl)
            {
                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    _fileService.DeleteImage(entity.ImageUrl);
                }
                entity.ImageUrl = product.ImageUrl;
            }

            if (product.Name != null)
                entity.Name = product.Name;
            if (product.Description != null)
                entity.Description = product.Description;
            if (product.Price.HasValue)
                entity.Price = product.Price.Value;
            if (product.Stock.HasValue)
                entity.Stock = product.Stock.Value;

            _uow.Products.Update(entity);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }

        private static ProductReadDto MapToReadDto(Product product)
        {
            return new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };
        }
    }
}
