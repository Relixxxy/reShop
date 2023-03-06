using System.Data;
using System.Runtime;
using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Order.Data;
using Order.Data.Entities;
using Order.Data.Repositories.Interfaces;
using Order.Host.Configurations;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services;

public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IInternalHttpClientService _httpClient;
    private readonly ILogger<OrderService> _logger;
    private readonly IOptions<OrderConfig> _settings;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IInternalHttpClientService httpClient,
        ILogger<OrderService> logger,
        IOptions<OrderConfig> settings,
        IMapper mapper,
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> baseServiceLogger)
        : base(dbContextWrapper, baseServiceLogger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _mapper = mapper;
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<int?> CreateOrderAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var response = await _httpClient.SendAsync<ItemsResponse<OrderProductDto>, ItemRequest<string>>(
                $"{_settings.Value.BasketUrl}/getproducts",
                HttpMethod.Post,
                new ItemRequest<string> { Item = userId });

            var count = response.Items.Count();
            _logger.LogInformation($"Received {count} items from basket");

            if (count <= 0)
            {
                throw new BusinessException("Can't create order with 0 items");
            }

            var orderNumber = GetRandomOrderNumber();

            var totalPrice = response.Items.Sum(p => p.Price * p.Amount);
            var productEntities = response.Items.Select(_mapper.Map<ProductEntity>).ToList();

            return await _orderRepository.CreateOrderAsync(userId, orderNumber, totalPrice, DateTime.Now.ToUniversalTime(), productEntities);
        });
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var orderEntities = await _orderRepository.GetOrdersByUserIdAsync(userId);
            var orders = orderEntities.Select(_mapper.Map<OrderDto>).ToList();
            return orders;
        });
    }

    private string GetRandomOrderNumber()
    {
        return $"{DateTime.Now:ddHHmmss}-{Random.Shared.Next(0, 999):000}-{Guid.NewGuid().ToString().Substring(0, 4)}";
    }
}
