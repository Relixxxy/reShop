using AutoMapper;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Enums;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MVC.ViewModels;

namespace MVC.UnitTests.Services;

public class CatalogServiceTests
{
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
    private readonly Mock<IHttpClientService> _httpClientServiceMock;
    private readonly Mock<ILogger<CatalogService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly CatalogService _catalogService;

    public CatalogServiceTests()
    {
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
        _httpClientServiceMock = new Mock<IHttpClientService>();
        _loggerMock = new Mock<ILogger<CatalogService>>();
        _mapperMock = new Mock<IMapper>();

        _appSettingsMock.Setup(x => x.Value).Returns(new AppSettings { CatalogUrl = "https://example.com" });

        _catalogService = new CatalogService(
            _httpClientServiceMock.Object,
            _loggerMock.Object,
            _appSettingsMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetCatalogItems_ReturnsProductsCatalogVM()
    {
        // Arrange
        var page = 1;
        var take = 10;
        var brand = "TestBrand";
        var type = "TestType";
        var expectedFilters = new Dictionary<ProductTypeFilter, string>()
        {
            { ProductTypeFilter.Brand, brand },
            { ProductTypeFilter.Type, type }
        };

        var expectedResponse = new ProductsCatalogVM();

        _httpClientServiceMock
            .Setup(x => x.SendAsync<ProductsCatalogVM, PaginatedItemsRequest<ProductTypeFilter>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<PaginatedItemsRequest<ProductTypeFilter>>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _catalogService.GetCatalogItems(page, take, brand, type);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.SendAsync<ProductsCatalogVM, PaginatedItemsRequest<ProductTypeFilter>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<PaginatedItemsRequest<ProductTypeFilter>>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Received {expectedResponse.Count} products from catalog")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetCatalogItems_WithoutFiltres_ReturnsProductsCatalogVM()
    {
        // Arrange
        var page = 1;
        var take = 10;

        var expectedResponse = new ProductsCatalogVM();

        _httpClientServiceMock
            .Setup(x => x.SendAsync<ProductsCatalogVM, PaginatedItemsRequest<ProductTypeFilter>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<PaginatedItemsRequest<ProductTypeFilter>>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _catalogService.GetCatalogItems(page, take, null, null);

        // Assert
        result.Should().Be(expectedResponse);
        _httpClientServiceMock.Verify(
            x => x.SendAsync<ProductsCatalogVM, PaginatedItemsRequest<ProductTypeFilter>>(
                It.IsAny<string>(),
                HttpMethod.Post,
                It.IsAny<PaginatedItemsRequest<ProductTypeFilter>>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Received {expectedResponse.Count} products from catalog")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetBrands_ShouldReturnListOfSelectListItems()
    {
        // Arrange
        var expectedBrands = new List<string> { "Brand 1", "Brand 2", "Brand 3" };
        _httpClientServiceMock
            .Setup(x => x.SendAsync<IEnumerable<string>, object>(
                It.Is<string>(url => url.Contains("/brands")),
                HttpMethod.Post,
                It.IsAny<object>()))
            .ReturnsAsync(expectedBrands);

        // Act
        var result = await _catalogService.GetBrands();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedBrands.Count);
        result.Should().BeAssignableTo<IEnumerable<SelectListItem>>();

        for (var i = 0; i < result.Count(); i++)
        {
            var brand = result.ElementAt(i);
            brand.Should().NotBeNull();
            brand.Value.Should().Be(expectedBrands[i]);
            brand.Text.Should().Be(expectedBrands[i]);
        }

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Received {expectedBrands.Count} brands from catalog")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetTypes_ShouldReturnListOfSelectListItems()
    {
        // Arrange
        var expectedTypes = new List<string> { "Type 1", "Type 2", "Type 3" };
        _httpClientServiceMock
            .Setup(x => x.SendAsync<IEnumerable<string>, object>(
                It.Is<string>(url => url.Contains("/types")),
                HttpMethod.Post,
                It.IsAny<object>()))
            .ReturnsAsync(expectedTypes);

        // Act
        var result = await _catalogService.GetTypes();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedTypes.Count);
        result.Should().BeAssignableTo<IEnumerable<SelectListItem>>();

        for (var i = 0; i < result.Count(); i++)
        {
            var type = result.ElementAt(i);
            type.Should().NotBeNull();
            type.Value.Should().Be(expectedTypes[i]);
            type.Text.Should().Be(expectedTypes[i]);
        }

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Received {expectedTypes.Count} types from catalog")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductById_Successful()
    {
        // Arrange
        int productId = 123;
        var productDto = new CatalogProductDto
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m
        };
        var itemResponse = new ItemResponse<CatalogProductDto>
        {
            Item = productDto,
        };

        _httpClientServiceMock
            .Setup(x => x.SendAsync<ItemResponse<CatalogProductDto>, IdRequest>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<IdRequest>()))
            .ReturnsAsync(itemResponse);

        _mapperMock.Setup(x => x.Map<ProductVM>(productDto))
            .Returns(new ProductVM
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            });

        // Act
        var result = await _catalogService.GetProductById(productId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ProductVM>();
        result.Id.Should().Be(productId);
        result.Name.Should().Be(productDto.Name);
        result.Description.Should().Be(productDto.Description);
        result.Price.Should().Be(productDto.Price);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Received product with id {result.Id} and mapped to VM")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductById_Failture()
    {
        // Arrange
        int productId = 123;
        ItemResponse<CatalogProductDto> itemResponse = null!;

        _httpClientServiceMock
            .Setup(x => x.SendAsync<ItemResponse<CatalogProductDto>, IdRequest>(
                It.IsAny<string>(),
                It.IsAny<HttpMethod>(),
                It.IsAny<IdRequest>()))
            .ReturnsAsync(itemResponse);

        // Act
        var result = await _catalogService.GetProductById(productId);

        // Assert
        result.Should().BeNull();

        _loggerMock.Verify(
           x => x.Log(
               LogLevel.Warning,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Received null")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }
}