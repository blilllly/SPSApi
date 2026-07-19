using SPSApi.Shared.Abstractions;

namespace SPSApi.Host;

public static class ModuleExtensions
{
  public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration config)
  {
    foreach (var module in ModuleLoader.DiscoverModules())
    {
      module.RegisterServices(services, config);
    }

    return services;
  }

  public static IEndpointRouteBuilder MapModules(this IEndpointRouteBuilder endpoints)
  {
    foreach (var module in ModuleLoader.DiscoverModules())
    {
      module.MapEndpoints(endpoints);
    }

    return endpoints;
  }

  // Run every module's migrations IN ORDER
  public static async Task MigrateModulesAsync(this IServiceProvider service, CancellationToken ct = default)
  {
    using var scope = service.CreateScope();

    var migrators = scope.ServiceProvider
      .GetServices<IModuleMigrator>()
      .OrderBy(m => m.Order)
      .ToList();

    foreach (var migrator in migrators)
    {
      await migrator.MigrateAsync();
    }
  }
}
