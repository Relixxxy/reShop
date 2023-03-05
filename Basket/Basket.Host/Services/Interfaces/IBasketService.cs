using Infrastructure.Models.Dtos;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddProductAsync(string userId, BasketProductDto product);
    Task RemoveProductAsync(string userId, int productId, int amount);
    Task<IEnumerable<BasketProductDto>> GetProductsAsync(string userId);
    Task ClearAsync(string userId);
}