using Infrastructure.Models.Dtos;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddProduct(string userId, BasketProductDto product);
    Task RemoveProduct(string userId, int productId, int amount);
    Task<IEnumerable<BasketProductDto>> GetProducts(string userId);
    Task Clear(string userId);
}