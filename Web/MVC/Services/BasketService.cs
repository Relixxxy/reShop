﻿using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class BasketService : IBasketService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<BasketService> _logger;

    public BasketService(IHttpClientService httpClient, ILogger<BasketService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task AddProduct(ProductVM product)
    {
        _logger.LogInformation(product.ToString());
        var result = await _httpClient.SendAsync<object, ItemRequest<ProductVM>>(
            $"{_settings.Value.BasketUrl}/addproduct",
            HttpMethod.Post,
            new ItemRequest<ProductVM> { Item = product });
    }

    public async Task<IEnumerable<ProductVM>> GetProducts()
    {
        var result = await _httpClient.SendAsync<ItemsResponse<ProductVM>, object>(
            $"{_settings.Value.BasketUrl}/getproducts",
            HttpMethod.Post,
            new { });

        _logger.LogInformation(result.ToString());

        return result.Items;
    }

    public async Task RemoveProduct(int id, int amount)
    {
        var result = await _httpClient.SendAsync<object, AmountProductRequest>(
            $"{_settings.Value.BasketUrl}/removeproduct",
            HttpMethod.Post,
            new AmountProductRequest { ProductId = id, Amount = amount });
    }
}
