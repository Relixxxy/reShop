using AutoMapper;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Response;
using Infrastructure.Models.Responses;
using Microsoft.Extensions.Options;
using MVC.ViewModels;

namespace MVC.UnitTests.Services;

public class OrderServiceTest
{
    private readonly Mock<IHttpClientService> _httpClientMock;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly Mock<IOptions<AppSettings>> _settingsMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IOrderService _orderService;

    public OrderServiceTest()
    {
        _httpClientMock = new Mock<IHttpClientService>();
        _loggerMock = new Mock<ILogger<OrderService>>();
        _settingsMock = new Mock<IOptions<AppSettings>>();
        _mapperMock = new Mock<IMapper>();

        _settingsMock.Setup(x => x.Value).Returns(new AppSettings { OrderUrl = "https://example.com" });

        _orderService = new OrderService(_httpClientMock.Object, _loggerMock.Object, _settingsMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_Successful()
    {
        // Arrange
        var expectedResult = 1;
        var addItemResponse = new AddItemResponse<int?> { Id = expectedResult };

        _httpClientMock.Setup(
            x => x.SendAsync<AddItemResponse<int?>, object>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<object>())).ReturnsAsync(addItemResponse);

        // Act
        var result = await _orderService.CreateOrderAsync();

        // Assert
        result.Should().Be(expectedResult);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Order created with id {expectedResult}")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_Failture()
    {
        // Arrange
        AddItemResponse<int?> addItemResponse = null!;

        _httpClientMock.Setup(x => x.SendAsync<AddItemResponse<int?>, object>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<object>())).ReturnsAsync(addItemResponse);

        // Act
        var result = await _orderService.CreateOrderAsync();

        // Assert
        result.Should().BeNull();
        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Order created with id {null}")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetOrdersAsync_Success()
    {
        // Arrange
        var orderDtos = new List<OrderDto> { new OrderDto { Id = 1 }, new OrderDto { Id = 2 } };
        var itemsResponse = new ItemsResponse<OrderDto> { Items = orderDtos };
        var orderVMs = orderDtos.Select(x => new OrderVM { Id = x.Id }).ToList();

        _httpClientMock.Setup(
            x => x.SendAsync<ItemsResponse<OrderDto>, object>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<object>()))
            .ReturnsAsync(itemsResponse);

        _mapperMock.Setup(x => x.Map<OrderVM>(It.IsAny<OrderDto>())).Returns<OrderDto>(orderDto => orderVMs.Find(x => x.Id == orderDto.Id) !);

        // Act
        var result = await _orderService.GetOrdersAsync();

        // Assert
        result.Should().BeEquivalentTo(orderVMs);

        _loggerMock.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((o, t) => o.ToString() !
                  .Contains($"Received {orderDtos.Count} orders from orderbff and mapped to vm")),
              It.IsAny<Exception>(),
              It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
          Times.Once);
    }

    [Fact]
    public async Task GetOrdersAsync_Failture()
    {
        // Arrange
        var orderDtos = new List<OrderDto>();
        var itemsResponse = new ItemsResponse<OrderDto> { Items = orderDtos };
        var orderVMs = orderDtos.Select(x => new OrderVM { Id = x.Id }).ToList();

        _httpClientMock.Setup(
            x => x.SendAsync<ItemsResponse<OrderDto>, object>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<object>()))
            .ReturnsAsync(itemsResponse);

        _mapperMock.Setup(x => x.Map<OrderVM>(It.IsAny<OrderDto>())).Returns<OrderDto>(orderDto => orderVMs.Find(x => x.Id == orderDto.Id) !);

        // Act
        var result = await _orderService.GetOrdersAsync();

        // Assert
        result.Should().BeEquivalentTo(orderVMs);

        _loggerMock.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((o, t) => o.ToString() !
                  .Contains($"Received {orderDtos.Count} orders from orderbff and mapped to vm")),
              It.IsAny<Exception>(),
              It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
          Times.Once);
    }
}
