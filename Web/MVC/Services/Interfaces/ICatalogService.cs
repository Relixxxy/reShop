using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<ProductsCatalogVM> GetCatalogItems(int page, int take, string? brand, string? type);
    Task<IEnumerable<SelectListItem>> GetBrands();
    Task<IEnumerable<SelectListItem>> GetTypes();
    Task<ProductVM> GetProductById(int productId);
}
