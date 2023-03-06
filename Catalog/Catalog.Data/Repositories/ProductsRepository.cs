using Catalog.Data.Entities;
using Catalog.Data.Repositories.Interfaces;
using Infrastructure.Exceptions;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data.Repositories;

public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _context;

    public ProductsRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _context = dbContextWrapper.DbContext;
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

        return result.Entity.Id;
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
            throw new BusinessException($"Product with id ({id}) doesn't exist");
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

        return true;
    }

    public async Task<IEnumerable<string>> GetTypesAsync()
    {
        var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
        return types;
    }

    public async Task<IEnumerable<string>> GetBrandsAsync()
    {
        var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        return brands;
    }
}
