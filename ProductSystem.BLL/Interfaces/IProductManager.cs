using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Shared;

namespace ProductSystem.BLL.Interfaces
{
    public interface IProductManager
    {
        Task<PaginatedResult<ProductReadDto>> GetProductsAsync(ProductQueryDto query);
        Task<ProductReadDto?> GetByIdAsync(int id);
        Task<Result<ProductReadDto>> AddAsync(ProductCreateDto product);
        Task<Result> UpdateAsync(int id, ProductUpdateDto product);
        Task<Result> DeleteAsync(int id);
    }
}
