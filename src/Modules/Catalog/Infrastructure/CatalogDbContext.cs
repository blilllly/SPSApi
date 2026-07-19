using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
  public DbSet<Brand> Brands => Set<Brand>();
  public DbSet<AssetModel> AssetModels => Set<AssetModel>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<ProductImage> ProductImages => Set<ProductImage>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("catalog");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
  }
}
