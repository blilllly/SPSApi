using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Infrastructure.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
  public void Configure(EntityTypeBuilder<Brand> b)
  {
    b.ToTable("Brand");
    b.HasKey(x => x.Id);
    b.Property(x => x.Name).HasMaxLength(100).IsRequired();
    b.HasIndex(x => x.Name).IsUnique();
  }
}
