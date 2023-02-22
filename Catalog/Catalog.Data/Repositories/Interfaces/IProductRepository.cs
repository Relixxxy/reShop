using Catalog.Data.Entities;

namespace Catalog.Data.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductEntity>> GetProductsByPageAsync(int pageIndex, int pageSize, string? brandFilter, string? typeFilter);
    Task<ProductEntity> GetProductByIdAsync(int id);
    Task<int?> AddProductAsync(string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand);
    Task<bool> UpdateProductAsync(int id, string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand);
    Task<bool> DeleteProductAsync(int id);
}
