using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(p => p.ID);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.IsActive).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired().ValueGeneratedOnAdd();

        builder.HasIndex(p => p.Name).IsUnique();
    }
}
