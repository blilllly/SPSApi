using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SPSApi.Modules.Customers.Domain;

namespace SPSApi.Modules.Customers.Infrastructure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
  public void Configure(EntityTypeBuilder<Contact> b)
  {
    b.ToTable("Contact");
    b.HasKey(x => x.Id);

    b.Property(x => x.Name).HasMaxLength(200).IsRequired();
    b.Property(x => x.Email).HasMaxLength(254);
    b.Property(x => x.Phone).HasMaxLength(30);
    b.Property(x => x.Position).HasMaxLength(100);

    b.HasOne(x => x.Customer)
      .WithMany()
      .HasForeignKey(x => x.CustomerId)
      .OnDelete(DeleteBehavior.Restrict);

    b.HasOne(x => x.Branch)
      .WithMany()
      .HasForeignKey(x => x.BranchId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}
