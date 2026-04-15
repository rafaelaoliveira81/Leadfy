using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class LeadConfig : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(l => l.Email)
               .HasMaxLength(254);

        builder.Property(l => l.PhoneNumber)
               .HasMaxLength(20);

        builder.Property(l => l.IsActive)
               .IsRequired();

        builder.Property(l => l.CreatedAt)
               .IsRequired()
               .ValueGeneratedOnAdd();
    }
}
