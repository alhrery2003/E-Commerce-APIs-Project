using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Interfaces;
using ProductSystem.BLL.Shared;
using ProductSystem.DAL.Entities;
using ProductSystem.DAL.UnitOfWork;

namespace ProductSystem.BLL.Managers
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _uow;

        public CartManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result> AddToCartAsync(string userId, AddToCartDto dto)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _uow.Carts.AddAsync(cart);
            }

            var product = await _uow.Products.GetByIdAsync(dto.ProductId);
            if (product == null) return Result.Failure("Product not found");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            var expectedQuantity = existingItem != null ? existingItem.Quantity + dto.Quantity : dto.Quantity;

            if (expectedQuantity > product.Stock)
            {
                return Result.Failure($"Insufficient stock. Only {product.Stock} available for '{product.Name}'.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity });
            }

            await _uow.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<CartDto>> GetUserCartAsync(string userId)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart == null) return Result<CartDto>.Failure("Cart not found");

            var dto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? string.Empty,
                    ProductPrice = i.Product?.Price ?? 0,
                    Quantity = i.Quantity
                }).ToList()
            };
            return Result<CartDto>.Success(dto);
        }

        public async Task<Result> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart == null) return Result.Failure("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return Result.Failure("Item not found in cart");

            cart.Items.Remove(item);
            await _uow.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateCartItemQuantityAsync(string userId, UpdateCartItemDto dto)
        {
            var cart = await _uow.Carts.GetCartWithItemsAsync(userId);
            if (cart == null) return Result.Failure("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item == null) return Result.Failure("Item not found in cart");

            if (dto.Quantity > 0)
            {
                var product = await _uow.Products.GetByIdAsync(dto.ProductId);
                if (product == null) return Result.Failure("Product not found");

                if (dto.Quantity > product.Stock)
                {
                    return Result.Failure($"Insufficient stock. Only {product.Stock} available for '{product.Name}'.");
                }

                item.Quantity = dto.Quantity;
            }
            else
            {
                cart.Items.Remove(item);
            }

            await _uow.SaveChangesAsync();
            return Result.Success();
        }
    }
}
