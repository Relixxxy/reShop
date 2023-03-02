using Infrastructure.Models.Requests;
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

    public OrderService(IHttpClientService httpClient, ILogger<OrderService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings;
    }

    public async Task<int?> CreateOrderAsync()
    {
        var result = await _httpClient.SendAsync<AddItemResponse<int?>, object>(
            $"{_settings.Value.OrderUrl}/createorder",
            HttpMethod.Post,
            new { });

        _logger.LogInformation($"Order created with id {result.Id}");

        return result.Id;
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var result = await _httpClient.SendAsync<ItemsResponse<Order>, object>(
            $"{_settings.Value.OrderUrl}/getorders",
            HttpMethod.Post,
            new { });

        _logger.LogInformation($"Received {result.Items.Count()} orders from orderbff");

        return result.Items;
    }
}
