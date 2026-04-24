using Microsoft.EntityFrameworkCore;
using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetUserOrdersWithItemsAsync(string userId)
        {
            return await _dbSet.Include(o => o.Items).ThenInclude(i => i.Product)
                               .Where(o => o.UserId == userId)
                               .OrderByDescending(o => o.OrderDate)
                               .ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsWithItemsAsync(string userId, int orderId)
        {
            return await _dbSet.Include(o => o.Items).ThenInclude(i => i.Product)
                               .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);
        }
    }
}
