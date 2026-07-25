using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Customers.Domain;

namespace SPSApi.Modules.Customers.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> b)
  {
    b.ToTable("Customer");
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(200).IsRequired();
    b.Property(x => x.TaxId).HasMaxLength(20);
    b.Property(x => x.IsActive).HasDefaultValue(true);

    b.HasIndex(x => x.Name).IsUnique();
  }
}
