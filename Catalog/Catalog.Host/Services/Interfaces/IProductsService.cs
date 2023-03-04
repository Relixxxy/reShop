using Infrastructure.Models.Dtos;

namespace Catalog.Host.Services.Interfaces;

public interface IProductsService
{
    Task<CatalogProductDto> GetProductAsync(int id);
    Task<int?> AddProductAsync(string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand);
    Task<bool> UpdateProductAsync(int id, string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand);
    Task<bool> DeleteProductAsync(int id);
}
