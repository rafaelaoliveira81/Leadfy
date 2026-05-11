using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class UserGroupPermissionConfig : IEntityTypeConfiguration<UserGroupPermission>
{
    public void Configure(EntityTypeBuilder<UserGroupPermission> builder)
    {
        builder.ToTable("UserGroupPermissions");
        builder.HasKey(gp => gp.ID);

        builder.Property(gp => gp.IsActive).IsRequired();
        builder.Property(gp => gp.CreatedAt).IsRequired().ValueGeneratedOnAdd();

        builder.HasOne(gp => gp.UserGroup)
               .WithMany(g => g.UserGroupPermissions)
               .HasForeignKey(gp => gp.UserGroupId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(gp => gp.Permission)
               .WithMany(p => p.UserGroupPermissions)
               .HasForeignKey(gp => gp.PermissionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(gp => new { gp.UserGroupId, gp.PermissionId }).IsUnique();
    }
}
