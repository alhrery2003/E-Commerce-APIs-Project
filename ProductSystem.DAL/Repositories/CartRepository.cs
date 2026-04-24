using Microsoft.EntityFrameworkCore;
using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartWithItemsAsync(string userId)
        {
            return await _dbSet.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
