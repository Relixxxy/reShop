using Order.Data.Entities;

namespace Order.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetOrdersByUserIdAsync(string userId);
    Task<int?> CreateOrderAsync(string userId, string orderNumber, decimal totalPrice, DateTime createdAt, IEnumerable<ProductEntity> products);
}
