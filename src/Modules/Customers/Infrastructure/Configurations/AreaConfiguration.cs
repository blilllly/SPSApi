using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Customers.Domain;

namespace SPSApi.Modules.Customers.Infrastructure.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
  public void Configure(EntityTypeBuilder<Area> b)
  {
    b.ToTable("Area");
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(100).IsRequired();

    b.HasOne(x => x.Branch)
      .WithMany()
      .HasForeignKey(x => x.BranchId)
      .OnDelete(DeleteBehavior.Cascade);

    b.HasIndex(x => new { x.BranchId, x.Name }).IsUnique();
  }
}
