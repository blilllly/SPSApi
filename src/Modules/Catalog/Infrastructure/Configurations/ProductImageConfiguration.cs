using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Catalog.Domain;

namespace SPSApi.Modules.Catalog.Infrastructure.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
  public void Configure(EntityTypeBuilder<ProductImage> b)
  {
    b.ToTable("ProductImage");
    b.HasKey(x => x.Id);

    b.Property(x => x.Url).HasMaxLength(1000).IsRequired();
    b.Property(x => x.SortOrder).HasDefaultValue(0);
    b.Property(x => x.IsPrimary).HasDefaultValue(false);

    b.HasOne(x => x.Product)
      .WithMany(p => p.Images)
      .HasForeignKey(x => x.ProductId)
      .OnDelete(DeleteBehavior.Cascade);

    b.HasIndex(x => new { x.ProductId, x.SortOrder });

    b.HasIndex(x => x.ProductId)
      .IsUnique()
      .HasFilter("[IsPrimary] = 1")
      .HasDatabaseName("UX_ProductImage_OnePrimaryPerProduct");
  }
}
