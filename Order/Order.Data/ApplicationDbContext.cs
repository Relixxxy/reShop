using Microsoft.EntityFrameworkCore;
using Order.Data.Entities;
using Order.Data.EntityConfigurations;

namespace Order.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderEntityConfiguration());
        builder.ApplyConfiguration(new ProductEntityConfiguration());
    }
}
