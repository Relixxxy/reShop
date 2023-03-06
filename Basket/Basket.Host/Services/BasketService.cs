using Basket.Host.Services.Interfaces;
using Infrastructure.Models.Dtos;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<BasketService> _logger;

    public BasketService(ICacheService cacheService, ILogger<BasketService> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task AddProductAsync(string userId, BasketProductDto product)
    {
        if (product.Amount <= 0)
        {
            return;
        }

        var products = (await GetProductsAsync(userId)).ToList();

        var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);

        if (existingProduct is not null)
        {
            existingProduct.Amount += product.Amount;
        }
        else
        {
            products.Add(product);
        }

        await _cacheService.AddOrUpdateAsync(userId, products);

        _logger.LogInformation($"Product {product.Name} added");
    }

    public async Task ClearAsync(string userId)
    {
        await _cacheService.ClearAsync(userId);
        _logger.LogInformation($"Cache cleared");
    }

    public async Task<IEnumerable<BasketProductDto>> GetProductsAsync(string userId)
    {
        var products = await _cacheService.GetAsync<IEnumerable<BasketProductDto>>(userId);

        if (products is null)
        {
            products = new List<BasketProductDto>();
        }

        _logger.LogInformation($"Found {products.Count()} products");

        return products;
    }

    public async Task RemoveProductAsync(string userId, int productId, int amount)
    {
        var products = await GetProductsAsync(userId);

        var existingProduct = products.FirstOrDefault(p => p.Id == productId);

        if (existingProduct is not null)
        {
            existingProduct.Amount -= amount;

            if (existingProduct.Amount <= 0)
            {
                products = products.Where(p => p.Id != productId);
            }

            await _cacheService.AddOrUpdateAsync(userId, products);

            _logger.LogInformation($"Product {existingProduct?.Name} removed");
        }
    }
}