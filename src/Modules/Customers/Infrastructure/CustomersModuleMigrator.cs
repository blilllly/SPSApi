using Microsoft.EntityFrameworkCore;
using SPSApi.Shared.Abstractions;

namespace SPSApi.Modules.Customers.Infrastructure;

public class CustomersModuleMigrator(CustomersDbContext db) : IModuleMigrator
{
  public int Order => 20;

  public Task MigrateAsync(CancellationToken ct = default) => db.Database.MigrateAsync(ct);
}
