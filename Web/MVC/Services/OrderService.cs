using AutoMapper;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Response;
using Infrastructure.Models.Responses;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class OrderService : IOrderService
{
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<OrderService> _logger;
    private readonly IOptions<AppSettings> _settings;
    private readonly IMapper _mapper;

    public OrderService(IHttpClientService httpClient, ILogger<OrderService> logger, IOptions<AppSettings> settings, IMapper mapper)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings;
        _mapper = mapper;
    }

    public async Task<int?> CreateOrderAsync()
    {
        var result = await _httpClient.SendAsync<AddItemResponse<int?>, object>(
            $"{_settings.Value.OrderUrl}/createorder",
            HttpMethod.Post,
            new { });

        var orderId = result?.Id;

        _logger.LogInformation($"Order created with id {orderId}");

        return orderId;
    }

    public async Task<IEnumerable<OrderVM>> GetOrdersAsync()
    {
        var result = await _httpClient.SendAsync<ItemsResponse<OrderDto>, object>(
            $"{_settings.Value.OrderUrl}/getorders",
            HttpMethod.Post,
            new { });

        var orders = result.Items.Select(_mapper.Map<OrderVM>);

        _logger.LogInformation($"Received {result.Items.Count()} orders from orderbff and mapped to vm");

        return orders;
    }
}
