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

    public async Task<int?> CreateOrderAsync(string userId, string orderNumber, decimal totalPrice, DateTime createdAt, IEnumerable<ProductEntity> products)
    {
        var order = new OrderEntity()
        {
            UserId = userId,
            OrderNumber = orderNumber,
            TotalPrice = totalPrice,
            CreatedAt = createdAt,
            Products = products
        };

        var result = await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<IEnumerable<OrderEntity>> GetOrdersByUserIdAsync(string userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Products)
            .ToListAsync();

        return orders;
    }
}
