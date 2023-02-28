using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Order.Data.Entities;
using Order.Data.Repositories.Interfaces;

namespace Order.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _context = dbContextWrapper.DbContext;
    }

    public async Task<int?> CreateOrder(int orderNumber, decimal totalPrice, DateTime createdAt, IEnumerable<ProductEntity> products)
    {
        var order = new OrderEntity()
        {
            OrderNumber = orderNumber,
            TotalPrice = totalPrice,
            CreatedAt = createdAt,
            Products = products
        };

        var result = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<IEnumerable<OrderEntity>> GetOrders()
    {
        var orders = await _context.Orders.Include(o => o.Products).ToListAsync();

        return orders;
    }
}
