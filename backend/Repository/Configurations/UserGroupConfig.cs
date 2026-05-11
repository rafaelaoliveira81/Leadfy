using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class UserGroupConfig : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.ToTable("UserGroups");
        builder.HasKey(g => g.ID);

        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.IsActive).IsRequired();
        builder.Property(g => g.CreatedAt).IsRequired().ValueGeneratedOnAdd();

        builder.HasIndex(g => g.Name).IsUnique();
    }
}
