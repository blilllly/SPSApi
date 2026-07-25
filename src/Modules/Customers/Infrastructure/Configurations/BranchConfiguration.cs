using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Customers.Domain;

namespace SPSApi.Modules.Customers.Infrastructure.Configurations;

public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
  public void Configure(EntityTypeBuilder<Branch> b)
  {
    b.ToTable("Branch");
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(200).IsRequired();
    b.Property(x => x.Address).HasMaxLength(500);
    b.Property(x => x.Latitude).HasPrecision(9, 6);
    b.Property(x => x.Longitude).HasPrecision(9, 6);
    b.Property(x => x.IsActive).HasDefaultValue(true);

    b.HasOne(x => x.Customer)
      .WithMany()
      .HasForeignKey(x => x.CustomerId)
      .OnDelete(DeleteBehavior.Restrict);

    b.HasIndex(x => new { x.CustomerId, x.Name }).IsUnique();
  }
}
