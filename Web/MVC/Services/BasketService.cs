using AutoMapper;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class BasketService : IBasketService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<BasketService> _logger;
    private readonly IMapper _mapper;

    public BasketService(IHttpClientService httpClient, ILogger<BasketService> logger, IOptions<AppSettings> settings, IMapper mapper)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task AddProduct(ProductVM product)
    {
        var productDto = _mapper.Map<BasketProductDto>(product);
        _logger.LogInformation($"Product with id {product.Id} mapped to dto");

        var result = await _httpClient.SendAsync<object, ItemRequest<BasketProductDto>>(
            $"{_settings.Value.BasketUrl}/addproduct",
            HttpMethod.Post,
            new ItemRequest<BasketProductDto> { Item = productDto });
    }

    public async Task<IEnumerable<ProductVM>> GetProducts()
    {
        var result = await _httpClient.SendAsync<ItemsResponse<BasketProductDto>, object>(
            $"{_settings.Value.BasketUrl}/getproducts",
            HttpMethod.Post,
            new { });

        if (result is null)
        {
            _logger.LogWarning("Recieved null from basketbff");
            return null!;
        }

        _logger.LogInformation($"Recieved {result.Items.Count()} products");

        var productsVM = result.Items.Select(_mapper.Map<ProductVM>);

        return productsVM;
    }

    public async Task RemoveProduct(int id, int amount)
    {
        var result = await _httpClient.SendAsync<object, AmountProductRequest>(
            $"{_settings.Value.BasketUrl}/removeproduct",
            HttpMethod.Post,
            new AmountProductRequest { ProductId = id, Amount = amount });
    }
}
