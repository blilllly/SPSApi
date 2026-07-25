using Microsoft.EntityFrameworkCore;
using SPSApi.Shared.Infrastructure;

namespace SPSApi.Modules.Customers.Infrastructure;

public class CustomersDbContextFactory : DesignTimeDbContextFactoryBase<CustomersDbContext>
{
  protected override string Schema => "customers";

  protected override void ConfigureProvider(
    DbContextOptionsBuilder<CustomersDbContext> builder,
    string connectionString,
    string schema) => builder.UseSqlServer(
      connectionString,
      sql => sql.MigrationsHistoryTable("__EFMigrationsHistory", schema));

  protected override CustomersDbContext CreateContext(DbContextOptions<CustomersDbContext> options)
    => new(options);
}
