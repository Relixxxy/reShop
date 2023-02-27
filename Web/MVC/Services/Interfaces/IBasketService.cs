using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface IBasketService
{
    Task AddProduct(Product product);
    Task RemoveProduct(int id, int amount);
    Task<IEnumerable<Product>> GetProducts();
}
