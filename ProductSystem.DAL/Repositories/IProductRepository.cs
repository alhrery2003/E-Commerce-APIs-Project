using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<Product?> GetByIdWithCategoryAsync(int id);
    }
}
