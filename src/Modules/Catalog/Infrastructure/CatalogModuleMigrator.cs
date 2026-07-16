using Microsoft.EntityFrameworkCore;
using SPSApi.Shared.Abstractions;

namespace SPSApi.Modules.Catalog.Infrastructure;

public class CatalogModuleMigrator(CatalogDbContext db) : IModuleMigrator
{
  public int Order => 30;

  public Task MigrateAsync(CancellationToken ct = default) => db.Database.MigrateAsync(ct);
}
