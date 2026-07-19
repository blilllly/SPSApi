using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Infrastructure.Configurations;

public class AssetModelConfiguration : IEntityTypeConfiguration<AssetModel>
{

  public void Configure(EntityTypeBuilder<AssetModel> b)
  {
    b.ToTable("AssetModel");
    b.HasKey(x => x.Id);
    b.Property(x => x.Name).HasMaxLength(150).IsRequired();
    b.Property(x => x.AssetType).HasConversion<byte>();
    b.Property(x => x.IsColour).HasDefaultValue(false);

    b.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
    b.HasIndex(x => new { x.BrandId, x.Name }).IsUnique();
  }

}