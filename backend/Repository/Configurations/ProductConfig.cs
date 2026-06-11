using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
       public void Configure(EntityTypeBuilder<Product> builder)
       {
              builder.ToTable("Products");

              builder.HasKey(p => p.Id);

              builder.Property(p => p.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(p => p.TenantId)
                     .IsRequired();

              builder.Property(p => p.Name)
                     .IsRequired()
                     .HasMaxLength(150);

              builder.Property(p => p.Description)
                     .HasMaxLength(1000);

              builder.Property(p => p.Price)
                     .IsRequired()
                     .HasColumnType("decimal(18,2)");

              builder.Property(p => p.IsActive)
                     .IsRequired();

              builder.Property(p => p.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();

              builder.HasIndex(p => p.TenantId);

              builder.HasIndex(p => new { p.TenantId, p.Name })
                     .IsUnique();

              builder.HasOne(p => p.Tenant)
                     .WithMany(t => t.Products)
                     .HasForeignKey(p => p.TenantId)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}
