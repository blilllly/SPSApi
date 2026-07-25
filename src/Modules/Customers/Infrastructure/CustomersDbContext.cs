using Microsoft.EntityFrameworkCore;
using SPSApi.Modules.Customers.Domain;

namespace SPSApi.Modules.Customers.Infrastructure;

public class CustomersDbContext(DbContextOptions<CustomersDbContext> options) : DbContext(options)
{
  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<Branch> Branches => Set<Branch>();
  public DbSet<Area> Areas => Set<Area>();
  public DbSet<Contact> Contacts => Set<Contact>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("customers");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);
  }
}
