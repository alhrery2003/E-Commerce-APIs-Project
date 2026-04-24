using ProductSystem.DAL.Repositories;

namespace ProductSystem.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }

        Task<int> SaveChangesAsync();
    }
}
