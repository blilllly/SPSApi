using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SPSApi.Shared.Infrastructure;

public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
  protected abstract string Schema { get; }

  protected abstract void ConfigureProvider(
      DbContextOptionsBuilder<TContext> builder, string connectionString, string schema);

  protected abstract TContext CreateContext(DbContextOptions<TContext> options);

  public TContext CreateDbContext(string[] args)
  {
    var connection = ResolveConnectionString();

    var builder = new DbContextOptionsBuilder<TContext>();
    ConfigureProvider(builder, connection, Schema);

    return CreateContext(builder.Options);
  }

  private static string ResolveConnectionString()
  {
    var hostPath = FindHostDirectory();

    var config = new ConfigurationBuilder()
      .SetBasePath(hostPath)
      .AddJsonFile("appsettings.json", optional: true)
      .AddJsonFile("appsettings.Development.json", optional: true)
      .AddEnvironmentVariables()
      .Build();

    return config.GetConnectionString("Default")
      ?? throw new InvalidOperationException
      (
        "No se encontró ConnectionStrings:Default. Revisa src/Host/appsettings.Development.json."
      );
  }

  private static string FindHostDirectory()
  {
    // Start at the current directory and search for src/Host/appsettings.Development.json
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

    while (dir is not null)
    {
      var candidate = Path.Combine(dir.FullName, "src", "Host");
      if (File.Exists(Path.Combine(candidate, "appsettings.Development.json")) ||
          File.Exists(Path.Combine(candidate, "appsettings.json")))
        return candidate;

      dir = dir.Parent;
    }

    // Fallback: current dir (dotnet ef usually runs from the solution root).
    return Directory.GetCurrentDirectory();
  }
}