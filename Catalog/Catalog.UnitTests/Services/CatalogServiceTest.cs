using AutoMapper;
using Catalog.Data;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Catalog.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Services;
using Catalog.Data.Entities;
using FluentAssertions;
using Infrastructure.Models.Dtos;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<IProductsRepository> _productsRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _baseServiceLogger;

    private readonly ProductEntity _testProduct = new ()
    {
        Id = 1,
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        Brand = "brand",
        Type = "type",
        PictureFileName = "1.png"
    };
    public CatalogServiceTest()
    {
        _productsRepository = new Mock<IProductsRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();
        _baseServiceLogger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_productsRepository.Object, _mapper.Object, _dbContextWrapper.Object, _logger.Object, _baseServiceLogger.Object);
    }

    [Fact]
    public async Task GetProductsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;
        var pagingPaginatedItemsSuccess = new PaginatedItems<ProductEntity>()
        {
            Data = new List<ProductEntity>()
            {
                new ProductEntity()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var productSuccess = new ProductEntity()
        {
            Name = "TestName"
        };

        var productDtoSuccess = new CatalogProductDto()
        {
            Name = "TestName"
        };

        _productsRepository.Setup(s => s.GetProductsByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<string?>(),
            It.IsAny<string?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogProductDto>(
            It.Is<ProductEntity>(i => i.Equals(productSuccess)))).Returns(productDtoSuccess);

        // act
        var result = await _catalogService.GetProductsAsync(testPageIndex, testPageSize, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);

        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"products")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        PaginatedItems<ProductEntity> item = null!;

        _productsRepository.Setup(s => s.GetProductsByPageAsync(
             It.Is<int>(i => i == testPageIndex),
             It.Is<int>(i => i == testPageSize),
             It.IsAny<string?>(),
             It.IsAny<string?>())).ReturnsAsync(item);

        // act
        var result = await _catalogService.GetProductsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();

        _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"Products not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductAsync_Success()
    {
        // arrange
        var id = _testProduct.Id;
        var testResult = new CatalogProductDto()
        {
            Id = id,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            Amount = 100,
            Brand = "brand",
            Type = "type",
            PictureUrl = "1.png"
        };

        _productsRepository.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(_testProduct);
        _mapper.Setup(s => s.Map<CatalogProductDto>(It.Is<ProductEntity>(i => i.Equals(_testProduct)))).Returns(testResult);

        // act
        var result = await _catalogService.GetProductAsync(id);

        // assert
        result.Should().Be(testResult);

        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() !
                    .Contains($"successfully got")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
            Times.Once);
    }

    [Fact]
    public async Task GetProductAsync_Failed()
    {
        // arrange
        var id = _testProduct.Id;
        CatalogProductDto testResult = null!;

        _productsRepository.Setup(s => s.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(_testProduct);
        _mapper.Setup(s => s.Map<CatalogProductDto>(It.Is<ProductEntity>(i => i.Equals(_testProduct)))).Returns(testResult);

        // act
        var result = await _catalogService.GetProductAsync(id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task GetBrandsAsync_Success()
    {
        // arrange
        var brands = new List<string>() { "brand1", "brand2" };

        _productsRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(brands);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().Contain(brands);
        _logger.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetBrandsAsync_Failed()
    {
        // arrange
        var expectedBrands = new List<string>() { "brand1", "brand2" };
        var actualBrands = new List<string>() { "brand3", "brand4" };

        _productsRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(actualBrands);

        // act
        var result = await _catalogService.GetBrandsAsync();

        // assert
        result.Should().NotContain(expectedBrands);
        _logger.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetTypesAsync_Success()
    {
        // arrange
        var types = new List<string>() { "type1", "type2" };

        _productsRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(types);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().Contain(types);
        _logger.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }

    [Fact]
    public async Task GetTypesAsync_Failed()
    {
        // arrange
        var expectedTypes = new List<string>() { "type1", "type2" };
        var actualTypes = new List<string>() { "type3", "type4" };

        _productsRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(actualTypes);

        // act
        var result = await _catalogService.GetTypesAsync();

        // assert
        result.Should().NotContain(expectedTypes);
        _logger.Verify(
           x => x.Log(
               LogLevel.Information,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((o, t) => o.ToString() !
                   .Contains($"Found")),
               It.IsAny<Exception>(),
               It.IsAny<Func<It.IsAnyType, Exception, string>>() !),
           Times.Once);
    }
}
