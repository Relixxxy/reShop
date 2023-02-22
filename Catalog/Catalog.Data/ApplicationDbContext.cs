using Catalog.Data.Entities;
using Catalog.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<ProductEntity> Products { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new ProductEntityConfiguration());
	}
}
