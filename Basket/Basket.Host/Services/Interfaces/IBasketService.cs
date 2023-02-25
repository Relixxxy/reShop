using Basket.Host.Models;
using Infrastructure.Models.Requests;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task AddProduct(string userId, Product product);
    Task RemoveProduct(string userId, AmountProductRequest request);
    Task<IEnumerable<Product>> GetProducts(string userId);
}