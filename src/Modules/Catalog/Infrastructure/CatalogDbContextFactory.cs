using Microsoft.EntityFrameworkCore;
using SPSApi.Shared.Infrastructure;

namespace SPSApi.Modules.Catalog.Infrastructure;

public class CatalogDbContextFactory : DesignTimeDbContextFactoryBase<CatalogDbContext>
{
  protected override string Schema => "catalog";

  protected override void ConfigureProvider(
    DbContextOptionsBuilder<CatalogDbContext> builder,
    string connectionString,
    string schema) => builder.UseSqlServer(
      connectionString,
      sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", schema));

  protected override CatalogDbContext CreateContext(DbContextOptions<CatalogDbContext> options)
    => new(options);
}
