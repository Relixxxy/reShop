using Catalog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.EntityConfigurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Product").HasKey(p => p.Id);
            builder.Property(p => p.Id).UseHiLo("product_hilo").IsRequired();

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.AvailableStock).IsRequired();
            builder.Property(p => p.PictureFileName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Type).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Brand).IsRequired().HasMaxLength(100);
        }
    }
}
