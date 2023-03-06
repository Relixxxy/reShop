using MVC.ViewModels;

namespace MVC.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderVM>> GetOrdersAsync();
    Task<int?> CreateOrderAsync();
}
