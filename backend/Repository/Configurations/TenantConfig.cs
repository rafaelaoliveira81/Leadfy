using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class TenantConfig : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
               .IsRequired(true)
               .HasMaxLength(150);

        builder.Property(t => t.CreatedAt)
               .IsRequired(true)
               .ValueGeneratedOnAdd();
    }
}