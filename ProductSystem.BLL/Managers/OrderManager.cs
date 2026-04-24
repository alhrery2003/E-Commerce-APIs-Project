using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Interfaces;
using ProductSystem.BLL.Shared;
using ProductSystem.DAL.Entities;
using ProductSystem.DAL.UnitOfWork;

namespace ProductSystem.BLL.Managers
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _uow;

        public OrderManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<OrderDto>> GetOrderDetailsAsync(string userId, int orderId)
        {
            var order = await _uow.Orders.GetOrderDetailsWithItemsAsync(userId, orderId);
            if (order == null) return Result<OrderDto>.Failure("Order not found");

            return Result<OrderDto>.Success(MapToDto(order));
        }

        public async Task<Result<IEnumerable<OrderDto>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _uow.Orders.GetUserOrdersWithItemsAsync(userId);
            return Result<IEnumerable<OrderDto>>.Success(orders.Select(MapToDto));
        }

        public async Task<Result<OrderDto>> PlaceOrderAsync(string userId)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart == null || !cart.Items.Any()) return Result<OrderDto>.Failure("Cart is empty");

            // Verify stock and deduct
            foreach (var item in cart.Items)
            {
                var product = await _uow.Products.GetByIdAsync(item.ProductId);
                if (product == null) return Result<OrderDto>.Failure($"Product with ID {item.ProductId} not found.");

                if (item.Quantity > product.Stock)
                {
                    return Result<OrderDto>.Failure($"Insufficient stock for '{product.Name}'. Only {product.Stock} available.");
                }

                product.Stock -= item.Quantity;
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Items = cart.Items.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product?.Price ?? 0
                }).ToList()
            };

            order.TotalAmount = order.Items.Sum(i => i.Quantity * i.UnitPrice);

            await _uow.Orders.AddAsync(order);

            // Clear cart
            cart.Items.Clear();

            await _uow.SaveChangesAsync();

            return Result<OrderDto>.Success(MapToDto(order));
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList()
            };
        }
    }
}
