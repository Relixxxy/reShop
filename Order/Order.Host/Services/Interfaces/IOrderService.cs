using Infrastructure.Models.Dtos;

namespace Order.Host.Services.Interfaces;

public interface IOrderService
{
    Task<int?> CreateOrderAsync(string userId);
    Task<IEnumerable<OrderDto>> GetOrdersAsync(string userId);
}
