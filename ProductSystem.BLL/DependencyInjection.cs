using Microsoft.Extensions.DependencyInjection;
using ProductSystem.BLL.Interfaces;
using ProductSystem.BLL.Managers;

namespace ProductSystem.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
