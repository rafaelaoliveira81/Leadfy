using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class OpportunityConfig : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.ToTable("Opportunities");

        builder.HasKey(o => o.ID);

        builder.Property(o => o.Title)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(o => o.LeadId)
               .IsRequired();

        builder.Property(o => o.OwnerId)
               .IsRequired(false);

        builder.Property(o => o.ProductId)
               .IsRequired();

        builder.Property(o => o.Stage)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(o => o.Amount)
               .IsRequired()
               .HasPrecision(18, 2);

        builder.Property(o => o.ExpectedCloseDate)
               .IsRequired();

        builder.Property(o => o.CreatedAt)
               .IsRequired()
               .ValueGeneratedOnAdd();

        builder.Property(o => o.IsActive)
               .IsRequired();

        builder.HasOne(o => o.Lead)
               .WithMany()
               .HasForeignKey(o => o.LeadId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Owner)
               .WithMany()
               .HasForeignKey(o => o.OwnerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Product)
               .WithMany()
               .HasForeignKey(o => o.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
