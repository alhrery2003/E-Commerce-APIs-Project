using ProductSystem.DAL.Repositories;

namespace ProductSystem.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public ICartRepository Carts { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext context, IProductRepository products, ICategoryRepository categories, ICartRepository carts, IOrderRepository orders)
        {
            _context = context;
            Products = products;
            Categories = categories;
            Carts = carts;
            Orders = orders;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
