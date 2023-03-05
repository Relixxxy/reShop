using Basket.Host.Services;
using Basket.Host.Services.Interfaces;
using FluentAssertions;
using Infrastructure.Models.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace Basket.Host.Tests.Services;

public class BasketServiceTests
{
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<ILogger<BasketService>> _loggerMock;
    private readonly BasketService _basketService;

    public BasketServiceTests()
    {
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<BasketService>>();

        _basketService = new BasketService(_cacheServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task AddProductAsync_NewProduct_ShouldAdd()
    {
        // Arrange
        var userId = "test_user_id";
        var product = new BasketProductDto { Id = 1, Name = "Product 1", Amount = 2 };
        var productsInCache = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 2, Name = "Product 2", Amount = 1 }
        };

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId))
            .ReturnsAsync(productsInCache);

        // Act
        await _basketService.AddProductAsync(userId, product);

        // Assert
        _cacheServiceMock.Verify(x => x.AddOrUpdateAsync(userId, It.Is<IEnumerable<BasketProductDto>>(p => p.Count() == 2)), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Product {product.Name} added")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task AddProductAsync_ExistingProduct_ShouldUpdate()
    {
        // Arrange
        var userId = "test_user_id";
        var product = new BasketProductDto { Id = 1, Name = "Product 1", Amount = 2 };
        var productsInCache = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 1, Name = "Product 1", Amount = 1 }
        };

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId))
            .ReturnsAsync(productsInCache);

        // Act
        await _basketService.AddProductAsync(userId, product);

        // Assert
        _cacheServiceMock.Verify(x => x.AddOrUpdateAsync(userId, It.Is<IEnumerable<BasketProductDto>>(p => p.First().Amount == 3)), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Product {product.Name} added")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task AddProductAsync_NegativeAmount_ShouldNotAddProduct()
    {
        // Arrange
        var userId = "test_user_id";
        var product = new BasketProductDto { Id = 1, Name = "Product 1", Amount = -1 };

        // Act
        await _basketService.AddProductAsync(userId, product);

        // Assert
        _cacheServiceMock.Verify(x => x.AddOrUpdateAsync(userId, It.IsAny<IEnumerable<BasketProductDto>>()), Times.Never);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Never);
    }

    [Fact]
    public async Task ClearAsync_Success()
    {
        // Arrange
        var userId = "test_user_id";

        // Act
        await _basketService.ClearAsync(userId);

        // Assert
        _cacheServiceMock.Verify(x => x.ClearAsync(userId), Times.Once);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Cache cleared")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_Failture()
    {
        // Arrange
        var userId = "test_user_id";
        List<BasketProductDto> productsInCache = null!;

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId)).ReturnsAsync(productsInCache);

        // Act
        var result = await _basketService.GetProductsAsync(userId);

        // Assert
        result.Should().BeEmpty();

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found {result.Count()} products")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_Success()
    {
        // Arrange
        var userId = "test_user_id";
        var productsInCache = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 1, Name = "Product 1", Amount = 2 },
            new BasketProductDto { Id = 2, Name = "Product 2", Amount = 1 }
        };
        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId))
            .ReturnsAsync(productsInCache);

        // Act
        var result = await _basketService.GetProductsAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(productsInCache);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found {result.Count()} products")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task RemoveProductAsync_AmountLessThanCurrent_ShouldNotRemoveProduct()
    {
        // Arrange
        var userId = "test_user_id";
        var productId = 1;
        var amount = 1;
        var productsInCache = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 1, Name = "Product 1", Amount = 2 },
            new BasketProductDto { Id = 2, Name = "Product 2", Amount = 1 }
        };

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId))
            .ReturnsAsync(productsInCache);

        // Act
        await _basketService.RemoveProductAsync(userId, productId, amount);

        // Assert
        _cacheServiceMock.Verify(x => x.AddOrUpdateAsync(userId, It.Is<IEnumerable<BasketProductDto>>(p => p.Count() == 2)), Times.Once);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Product {productsInCache.First().Name} removed")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task RemoveProductAsync_AmountEqualToCurrent_ShouldRemoveProduct()
    {
        // Arrange
        var userId = "test_user_id";
        var productId = 1;
        var amount = 2;
        var productsInCache = new List<BasketProductDto>
        {
            new BasketProductDto { Id = 1, Name = "Product 1", Amount = 2 },
            new BasketProductDto { Id = 2, Name = "Product 2", Amount = 1 }
        };

        _cacheServiceMock.Setup(x => x.GetAsync<IEnumerable<BasketProductDto>>(userId))
            .ReturnsAsync(productsInCache);

        // Act
        await _basketService.RemoveProductAsync(userId, productId, amount);

        // Assert
        _cacheServiceMock.Verify(x => x.AddOrUpdateAsync(userId, It.Is<IEnumerable<BasketProductDto>>(p => p.Count() == 1)), Times.Once);

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Product {productsInCache.First(p => p.Id == productId).Name} removed")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }
}
