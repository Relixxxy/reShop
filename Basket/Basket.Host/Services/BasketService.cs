using Basket.Host.Models;
using Basket.Host.Models.Requests;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ICacheService _cacheService;

    public BasketService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task AddProduct(string userId, Product product)
    {
        if (product.AvailableStock <= 0)
        {
            return;
        }

        var products = (await GetProducts(userId)).ToList();

        var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);

        if (existingProduct != null)
        {
            existingProduct.AvailableStock += product.AvailableStock;
        }
        else
        {
            products.Add(product);
        }

        await _cacheService.AddOrUpdateAsync(userId, products);
    }

    public async Task<IEnumerable<Product>> GetProducts(string userId)
    {
        var products = await _cacheService.GetAsync<IEnumerable<Product>>(userId);

        if (products is null)
        {
            return new List<Product>();
        }

        return products;
    }

    public async Task RemoveProduct(string userId, AmountProductRequest request)
    {
        var products = await GetProducts(userId);

        var existingProduct = products.FirstOrDefault(p => p.Id == request.ProductId);

        if (existingProduct is not null)
        {
            existingProduct.AvailableStock -= request.Amount;

            if (existingProduct.AvailableStock <= 0)
            {
                products = products.Where(p => p.Id != request.ProductId);
            }
        }

        await _cacheService.AddOrUpdateAsync(userId, products);
    }
}