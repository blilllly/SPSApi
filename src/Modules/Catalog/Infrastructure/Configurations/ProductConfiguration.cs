using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> b)
  {
    b.ToTable("Product");
    b.HasKey(x => x.Id);

    b.Property(x => x.Sku).HasMaxLength(60).IsRequired();
    b.Property(x => x.Name).HasMaxLength(200).IsRequired();
    b.Property(x => x.Category).HasConversion<byte>();
    b.Property(x => x.UnitOfMeasure).HasMaxLength(10).HasDefaultValue("EA");
    b.Property(x => x.Barcode).HasMaxLength(60);
    b.Property(x => x.PartNumber).HasMaxLength(80);
    b.Property(x => x.IsOriginal).HasDefaultValue(false);
    b.Property(x => x.IsActive).HasDefaultValue(true);
    b.Property(x => x.EstimatedCost).HasPrecision(14, 2);

    b.HasIndex(x => x.Sku).IsUnique();
    b.HasIndex(x => x.Barcode).IsUnique(false).HasFilter("[Barcode] IS NOT NULL");
    b.HasIndex(x => x.PartNumber).IsUnique(false).HasFilter("[PartNumber] IS NOT NULL");
  }
}
