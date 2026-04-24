using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersWithItemsAsync(string userId);
        Task<Order?> GetOrderDetailsWithItemsAsync(string userId, int orderId);
    }
}
