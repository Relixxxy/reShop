using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface IBasketService
{
    Task AddProduct(ProductVM product);
    Task RemoveProduct(int id, int amount);
    Task<IEnumerable<ProductVM>> GetProducts();
}
