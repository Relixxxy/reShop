using Order.Data.Entities;

namespace Order.Data.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderEntity>> GetOrders();
    Task<int?> CreateOrder(int orderNumber, decimal totalPrice, DateTime createdAt, IEnumerable<ProductEntity> products);
}
