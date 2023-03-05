using AutoMapper;
using Infrastructure.Models.Enums;
using Infrastructure.Models.Requests;
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
        var expectedUrl = $"{_appSettingsMock.Object.Value.CatalogUrl}/products";
        var expectedRequest = new PaginatedItemsRequest<ProductTypeFilter>()
        {
            PageIndex = page,
            PageSize = take,
            Filters = expectedFilters
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
}