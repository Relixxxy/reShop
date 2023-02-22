using Catalog.Data.Entities;
using Catalog.Data.Repositories.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper, ILogger<ProductRepository> logger)
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

    public async Task<IEnumerable<ProductEntity>> GetProductsByPageAsync(int pageIndex, int pageSize, string? brandFilter, string? typeFilter)
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

        var products = await query.OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        _logger.LogInformation($"Found {products.Count} products");

        return products;
    }

    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync();

        if (product is null)
        {
            _logger.LogWarning($"Product with id ({id}) doesn't exist");
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
