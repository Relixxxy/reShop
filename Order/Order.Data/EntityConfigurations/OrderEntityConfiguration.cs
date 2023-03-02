using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Data.Entities;

namespace Order.Data.EntityConfigurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Order").HasKey(p => p.Id);
        builder.Property(p => p.Id).UseHiLo("order_hilo").IsRequired();

        builder.Property(p => p.OrderNumber).IsRequired();
        builder.Property(p => p.TotalPrice).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
    }
}
