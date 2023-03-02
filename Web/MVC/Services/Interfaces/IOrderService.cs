using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<int?> CreateOrderAsync();
}
