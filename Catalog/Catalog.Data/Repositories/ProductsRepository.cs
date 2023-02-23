﻿using Catalog.Data.Entities;
using Catalog.Data.Repositories.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Data.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductsRepository> _logger;

    public ProductsRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<ProductsRepository> logger)
    {
        _context = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> AddProductAsync(string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand)
    {
        var product = new ProductEntity()
        {
            Name = name,
            Description = desc,
            Price = price,
            AvailableStock = availableStock,
            PictureFileName = pictureName,
            Type = type,
            Brand = brand
        };

        var result = await _context.AddAsync(product);

        await _context.SaveChangesAsync();

        var id = result.Entity.Id;
        _logger.LogInformation($"Product with id ({id}) has added");

        return id;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await GetProductByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Product with id ({id}) has deleted");

        return true;
    }

    public async Task<PaginatedItems<ProductEntity>> GetProductsByPageAsync(int pageIndex, int pageSize, string? brandFilter, string? typeFilter)
    {
        IQueryable<ProductEntity> query = _context.Products;

        if (brandFilter is not null)
        {
            query = query.Where(p => p.Brand == brandFilter);
        }

        if (typeFilter is not null)
        {
            query = query.Where(p => p.Type == typeFilter);
        }

        var totalItems = await query.LongCountAsync();

        var products = await query.OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        _logger.LogInformation($"Found {products.Count} products");

        return new PaginatedItems<ProductEntity>()
        {
            Data = products,
            TotalCount = totalItems,
        };
    }

    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            string message = $"Product with id ({id}) doesn't exist";
            _logger.LogWarning(message);
            throw new BusinessException(message);
        }

        return product!;
    }

    public async Task<bool> UpdateProductAsync(int id, string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand)
    {
        var product = await GetProductByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        product.Name = name;
        product.Description = desc;
        product.Price = price;
        product.AvailableStock = availableStock;
        product.PictureFileName = pictureName;
        product.Type = type;
        product.Brand = brand;

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Product with id ({id}) has updated");

        return true;
    }
}