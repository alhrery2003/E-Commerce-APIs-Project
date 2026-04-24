using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Shared;

namespace ProductSystem.BLL.Interfaces
{
    public interface ICartManager
    {
        Task<Result<CartDto>> GetUserCartAsync(string userId);
        Task<Result> AddToCartAsync(string userId, AddToCartDto dto);
        Task<Result> UpdateCartItemQuantityAsync(string userId, UpdateCartItemDto dto);
        Task<Result> RemoveFromCartAsync(string userId, int productId);
    }

    public interface IOrderManager
    {
        Task<Result<OrderDto>> PlaceOrderAsync(string userId);
        Task<Result<IEnumerable<OrderDto>>> GetUserOrdersAsync(string userId);
        Task<Result<OrderDto>> GetOrderDetailsAsync(string userId, int orderId);
    }
}
