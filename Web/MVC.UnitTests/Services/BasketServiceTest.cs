using AutoMapper;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using Microsoft.Extensions.Options;
using MVC.ViewModels;

namespace MVC.UnitTests.Services;

public class BasketServiceTest
{
    private readonly Mock<IHttpClientService> _httpClientServiceMock;
    private readonly Mock<ILogger<BasketService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;

    private readonly BasketService _basketService;

    public BasketServiceTest()
    {
        _httpClientServiceMock = new Mock<IHttpClientService>();
        _loggerMock = new Mock<ILogger<BasketService>>();
        _mapperMock = new Mock<IMapper>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();

        _appSettingsMock.Setup(x => x.Value).Returns(new AppSettings { BasketUrl = "https://example.com" });

        _basketService = new BasketService(
            _httpClientServiceMock.Object,
            _loggerMock.Object,
            _appSettingsMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task AddProduct_Successful()
    {
        // Arrange
        var product = new ProductVM { Id = 1, Name = "Product 1", Price = 10.0m };
        var productDto = new BasketProductDto { Id = 1, Name = "Product 1", Price = 10.0m };

        _mapperMock.Setup(x => x.Map<BasketProductDto>(product)).Returns(productDto);

        // Act
        await _basketService.AddProduct(product);

        // Assert
        _mapperMock.Verify(x => x.Map<BasketProductDto>(product), Times.Once);

        _httpClientServiceMock.Verify(
            x => x.SendAsync<object, ItemRequest<BasketProductDto>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.Is<ItemRequest<BasketProductDto>>(r => r.Item == productDto)),
            Times.Once);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Product with id {product.Id} mapped to dto")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetProducts_Successful()
    {
        // Arrange
        var productDtos = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 1, Name = "Product 1", Price = 10.0m },
            new BasketProductDto { Id = 2, Name = "Product 2", Price = 20.0m },
            new BasketProductDto { Id = 3, Name = "Product 3", Price = 30.0m }
        };
        var itemsResponse = new ItemsResponse<BasketProductDto> { Items = productDtos };
        _httpClientServiceMock.Setup(x => x.SendAsync<ItemsResponse<BasketProductDto>, object>(
            It.IsAny<string>(),
            HttpMethod.Post,
            It.IsAny<object>())).ReturnsAsync(itemsResponse);

        var expectedProductsVM = productDtos.Select(_mapperMock.Object.Map<ProductVM>);

        // Act
        var productsVM = await _basketService.GetProducts();

        // Assert
        _httpClientServiceMock.Verify(
            x => x.SendAsync<ItemsResponse<BasketProductDto>, object>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<object>()),
            Times.Once);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Recieved {itemsResponse.Items.Count()} products")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);

        productsVM.Should().BeEquivalentTo(expectedProductsVM);
    }

    [Fact]
    public async Task GetProducts_Failture()
    {
        // Arrange
        ItemsResponse<BasketProductDto> itemsResponse = null!;

        _httpClientServiceMock.Setup(x => x.SendAsync<ItemsResponse<BasketProductDto>, object>(
            It.IsAny<string>(),
            HttpMethod.Post,
            It.IsAny<object>())).ReturnsAsync(itemsResponse);

        // Act
        var productsVM = await _basketService.GetProducts();

        // Assert
        _httpClientServiceMock.Verify(
            x => x.SendAsync<ItemsResponse<BasketProductDto>, object>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<object>()),
            Times.Once);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Warning,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains("Recieved null from basketbff")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);

        productsVM.Should().BeNull();
    }

    [Fact]
    public async Task RemoveProduct_Successful()
    {
        // Arrange
        int productId = 123;
        int amount = 2;

        _httpClientServiceMock
            .Setup(x => x.SendAsync<object, AmountProductRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<AmountProductRequest>()))
            .ReturnsAsync(new object());

        // Act
        await _basketService.RemoveProduct(productId, amount);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.SendAsync<object, AmountProductRequest>(It.IsAny<string>(), HttpMethod.Post, It.IsAny<AmountProductRequest>()),
            Times.Once);
    }
}
