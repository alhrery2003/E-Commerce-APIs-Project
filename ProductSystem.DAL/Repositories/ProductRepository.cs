using Microsoft.EntityFrameworkCore;
using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _dbSet.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int id)
        {
            return await _dbSet.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
