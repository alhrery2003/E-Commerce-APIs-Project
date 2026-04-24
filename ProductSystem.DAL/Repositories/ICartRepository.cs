using ProductSystem.DAL.Entities;

namespace ProductSystem.DAL.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsAsync(string userId);
    }
}
