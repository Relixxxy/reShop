using AutoMapper;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Order.Data;
using Order.Data.Entities;
using Order.Data.Repositories.Interfaces;
using Order.Host.Configurations;
using Order.Host.Services;

namespace Order.UnitTests.Services;

public class OrderServiceTest
{
    private readonly OrderService _orderService;

    private readonly Mock<IInternalHttpClientService> _httpClientMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly Mock<IOptions<OrderConfig>> _settingsMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _baseServiceLogger;

    public OrderServiceTest()
    {
        _httpClientMock = new Mock<IInternalHttpClientService>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _loggerMock = new Mock<ILogger<OrderService>>();
        _settingsMock = new Mock<IOptions<OrderConfig>>();
        _mapperMock = new Mock<IMapper>();

        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _baseServiceLogger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _settingsMock.Setup(x => x.Value).Returns(new OrderConfig { BasketUrl = "https://example.com" });

        _orderService = new OrderService(
            _orderRepositoryMock.Object,
            _httpClientMock.Object,
            _loggerMock.Object,
            _settingsMock.Object,
            _mapperMock.Object,
            _dbContextWrapper.Object,
            _baseServiceLogger.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_UserHaveProducts_ReturnOrderId()
    {
        // Arrange
        var userId = "alice";
        var productDtos = new List<OrderProductDto>
        {
            new OrderProductDto { Id = 1, Price = 10.0m, Amount = 2 },
            new OrderProductDto { Id = 2, Price = 20.0m, Amount = 1 },
        };
        var response = new ItemsResponse<OrderProductDto> { Items = productDtos };

        _httpClientMock.Setup(
            x => x.SendAsync<ItemsResponse<OrderProductDto>, ItemRequest<string>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<ItemRequest<string>>())).ReturnsAsync(response);

        _orderRepositoryMock.Setup(x => x.CreateOrderAsync(userId, It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<List<ProductEntity>>())).ReturnsAsync(1);

        // Act
        var result = await _orderService.CreateOrderAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(1);
    }

    [Fact]
    public async Task CreateOrderAsync_UserHaveNoProducts_ThrowBusinessException()
    {
        // Arrange
        var userId = "alice";
        var response = new ItemsResponse<OrderProductDto> { Items = new List<OrderProductDto>() };
        _httpClientMock.Setup(x => x.SendAsync<ItemsResponse<OrderProductDto>, ItemRequest<string>>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<ItemRequest<string>>()))
            .ReturnsAsync(response);

        // Act
        Func<Task> action = async () => await _orderService.CreateOrderAsync(userId);

        // Assert
        await action.Should().ThrowAsync<BusinessException>().WithMessage("Can't create order with 0 items");
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsOrders()
    {
        // Arrange
        var userId = "alice";
        var orderEntities = new List<OrderEntity>
        {
            new OrderEntity { Id = 1 },
            new OrderEntity { Id = 2 },
            new OrderEntity { Id = 3 }
        };
        var orderDtos = new List<OrderDto>
        {
            new OrderDto { Id = 1 },
            new OrderDto { Id = 2 },
            new OrderDto { Id = 3 }
        };

        _orderRepositoryMock.Setup(x => x.GetOrdersByUserIdAsync(userId)).ReturnsAsync(orderEntities);
        _mapperMock.Setup(x => x.Map<OrderDto>(It.IsAny<OrderEntity>()))
           .Returns<OrderEntity>(orderEntity => orderDtos.Find(x => x.Id == orderEntity.Id) !);

        // Act
        var result = await _orderService.GetOrdersAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(orderDtos.Count);
        result.Should().BeEquivalentTo(orderDtos);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsEmptyList()
    {
        // Arrange
        var userId = "alice";
        var orderEntities = new List<OrderEntity>();
        _orderRepositoryMock.Setup(x => x.GetOrdersByUserIdAsync(userId)).ReturnsAsync(orderEntities);

        // Act
        var result = await _orderService.GetOrdersAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
