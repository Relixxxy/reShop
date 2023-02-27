using Basket.Host.Models;
using Infrastructure.Models.Requests;
using Basket.Host.Services.Interfaces;

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

    public async Task AddProduct(string userId, Product product)
    {
        if (product.Amount <= 0)
        {
            return;
        }

        var products = (await GetProducts(userId)).ToList();

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

    public async Task<IEnumerable<Product>> GetProducts(string userId)
    {
        var products = await _cacheService.GetAsync<IEnumerable<Product>>(userId);

        if (products is null)
        {
            products = new List<Product>();
        }

        _logger.LogInformation($"Found {products.Count()} products");

        return products;
    }

    public async Task RemoveProduct(string userId, AmountProductRequest request)
    {
        var products = await GetProducts(userId);

        var existingProduct = products.FirstOrDefault(p => p.Id == request.ProductId);

        if (existingProduct is not null)
        {
            existingProduct.Amount -= request.Amount;

            if (existingProduct.Amount <= 0)
            {
                products = products.Where(p => p.Id != request.ProductId);
            }

            await _cacheService.AddOrUpdateAsync(userId, products);

            _logger.LogInformation($"Product {existingProduct?.Name} removed");
        }
    }
}