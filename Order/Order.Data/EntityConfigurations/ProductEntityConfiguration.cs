using Order.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Data.EntityConfigurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Product").HasKey(p => p.Id);
            builder.Property(p => p.Id).UseHiLo("product_hilo").IsRequired();

            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Amount).IsRequired();

            builder.HasOne<OrderEntity>()
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.OrderId);
        }
    }
}
