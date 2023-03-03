using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<ProductsCatalog> GetCatalogItems(int page, int take, string? brand, string? type);
    Task<IEnumerable<SelectListItem>> GetBrands();
    Task<IEnumerable<SelectListItem>> GetTypes();
    Task<Product> GetProductById(int productId);
}
