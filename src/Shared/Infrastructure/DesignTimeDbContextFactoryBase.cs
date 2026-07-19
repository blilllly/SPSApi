using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SPSApi.Shared.Infrastructure;

public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
  where TContext : DbContext
{

  private const string LocalDockerConnection =
    "Server=localhost,1433;Database=SPSApi;User Id=sa;Password=Sps_Dev_2026!;" +
    "TrustServerCertificate=True;Encrypt=True"
  ;

  protected abstract string Schema { get; }

  protected abstract void ConfigureProvider(
    DbContextOptionsBuilder<TContext> builder, string connectionString, string schema
  );

  protected abstract TContext CreateContext(DbContextOptions<TContext> options);
  public TContext CreateDbContext(string[] args)
  {
    var connection =
      Environment.GetEnvironmentVariable("SPSAPI_DESIGN_CONNECTION")
      ?? LocalDockerConnection;

    var builder = new DbContextOptionsBuilder<TContext>();
    ConfigureProvider(builder, connection, Schema);

    return CreateContext(builder.Options);
  }
}
