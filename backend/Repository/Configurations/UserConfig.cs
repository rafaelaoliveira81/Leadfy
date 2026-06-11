using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(p => p.Id)
                     .ValueGeneratedOnAdd();
        
        builder.Property(u => u.Name)
                .IsRequired(true)
                .HasMaxLength(150);

        builder.Property(u => u.UserName)
                .IsRequired(true)
                .HasMaxLength(150);
        
        builder.Property(u => u.Email)
                .IsRequired(true)
                .HasMaxLength(254);
        
        builder.Property(u => u.PasswordHash)
                .IsRequired(true)
                .HasMaxLength(255);

        builder.Property(u => u.TenantId)
                .IsRequired(true);
        
        builder.Property(u => u.IsActive)
                .IsRequired(true);
        
        builder.Property(u => u.CreatedAt)
                .IsRequired(true)
                .ValueGeneratedOnAdd();

                builder.HasIndex(u => u.TenantId);

                builder.HasIndex(u => u.UserName).IsUnique();

                builder.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();

                builder.HasOne(u => u.Tenant)
                           .WithMany(t => t.Users)
                           .HasForeignKey(u => u.TenantId)
                           .OnDelete(DeleteBehavior.Restrict);
    }
}