using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class OwerConfig : IEntityTypeConfiguration<Ower>
{
    public void Configure(EntityTypeBuilder<Ower> builder)
    {
        builder.ToTable("Owers");

        builder.HasKey(o => o.ID);

        builder.Property(o => o.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(o => o.IsActive)
               .IsRequired();

        builder.Property(o => o.CreatedAt)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.HasOne(o => o.User)
               .WithMany(u => u.Owers)
               .HasForeignKey(o => o.UserID)
               .OnDelete(DeleteBehavior.Restrict);
    }
}