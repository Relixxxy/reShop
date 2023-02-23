﻿using AutoMapper;
using Catalog.Data;
using Catalog.Data.Entities;
using Catalog.Data.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests.Services;

public class ProductServiceTest
{
    private readonly IProductsService _productsService;

    private readonly Mock<IProductsRepository> _productsRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<ProductsService>> _logger;
    private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _baseServiceLogger;

    private readonly ProductEntity _testProduct = new ()
    {
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        Brand = "brand",
        Type = "type",
        PictureFileName = "1.png"
    };

    public ProductServiceTest()
    {
        _productsRepository = new Mock<IProductsRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<ProductsService>>();
        _baseServiceLogger = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(dbContextTransaction.Object);

        _productsService = new ProductsService(_productsRepository.Object, _mapper.Object, _dbContextWrapper.Object, _logger.Object, _baseServiceLogger.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;

        _productsRepository.Setup(s => s.AddProductAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.AddProductAsync(_testProduct.Name, _testProduct.Description, _testProduct.Price, _testProduct.AvailableStock, _testProduct.Brand, _testProduct.Type, _testProduct.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;

        _productsRepository.Setup(s => s.AddProductAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.AddProductAsync(_testProduct.Name, _testProduct.Description, _testProduct.Price, _testProduct.AvailableStock, _testProduct.Brand, _testProduct.Type, _testProduct.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var testResult = true;

        _productsRepository.Setup(s => s.UpdateProductAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.UpdateProductAsync(_testProduct.Id, _testProduct.Name, _testProduct.Description, _testProduct.Price, _testProduct.AvailableStock, _testProduct.Brand, _testProduct.Type, _testProduct.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        var testResult = false;

        _productsRepository.Setup(s => s.UpdateProductAsync(
             It.IsAny<int>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<decimal>(),
             It.IsAny<int>(),
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.UpdateProductAsync(_testProduct.Id, _testProduct.Name, _testProduct.Description, _testProduct.Price, _testProduct.AvailableStock, _testProduct.Brand, _testProduct.Type, _testProduct.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testResult = true;

        _productsRepository.Setup(s => s.DeleteProductAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.DeleteProductAsync(_testProduct.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        var testResult = false;

        _productsRepository.Setup(s => s.DeleteProductAsync(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _productsService.DeleteProductAsync(_testProduct.Id);

        // assert
        result.Should().Be(testResult);
    }
}
