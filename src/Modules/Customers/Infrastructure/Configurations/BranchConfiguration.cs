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
    b.Property(x => x.IsActive).HasDefaultValue(true);

    b.OwnsOne(x => x.Address, a =>
    {
      a.Property(x => x.MainStreet).HasColumnName("MainStreet").HasMaxLength(200).IsRequired();
      a.Property(x => x.SecondaryStreet).HasColumnName("SecondaryStreet").HasMaxLength(200);
      a.Property(x => x.BuildingNumber).HasColumnName("BuildingNumber").HasMaxLength(50);
      a.Property(x => x.City).HasColumnName("City").HasMaxLength(100);
      a.Property(x => x.Province).HasColumnName("Province").HasMaxLength(100).IsRequired();
      a.Property(x => x.PostalCode).HasColumnName("PostalCode").HasMaxLength(20);
      a.Property(x => x.Reference).HasColumnName("Reference").HasMaxLength(300);
      a.Property(x => x.Latitude).HasColumnName("Latitude").HasPrecision(9, 6);
      a.Property(x => x.Longitude).HasColumnName("Longitude").HasPrecision(9, 6);
    });
    b.Navigation(x => x.Address).IsRequired();

    b.HasOne(x => x.Customer)
      .WithMany()
      .HasForeignKey(x => x.CustomerId)
      .OnDelete(DeleteBehavior.Restrict);

    b.HasIndex(x => new { x.CustomerId, x.Name }).IsUnique();
  }
}
