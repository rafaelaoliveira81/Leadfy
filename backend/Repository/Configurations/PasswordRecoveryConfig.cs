using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class PasswordRecoveryConfig : IEntityTypeConfiguration<PasswordRecovery>
{
    public void Configure(EntityTypeBuilder<PasswordRecovery> builder)
    {
        builder.ToTable("PasswordRecoveries");
        builder.HasKey(pr => pr.ID);

        builder.Property(pr => pr.Email).IsRequired().HasMaxLength(254);
        builder.Property(pr => pr.Token).IsRequired().HasMaxLength(36);
        builder.Property(pr => pr.CreatedAt).IsRequired().ValueGeneratedOnAdd();
        builder.Property(pr => pr.ExpiresAt).IsRequired();
        builder.Property(pr => pr.IsActive).IsRequired();

        builder.HasOne(pr => pr.User)
               .WithMany(u => u.PasswordRecoveries)
               .HasForeignKey(pr => pr.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pr => pr.Token).IsUnique();
    }
}
