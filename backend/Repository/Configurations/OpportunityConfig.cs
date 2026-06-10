using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class OpportunityConfig : IEntityTypeConfiguration<Opportunity>
{
       public void Configure(EntityTypeBuilder<Opportunity> builder)
       {
              builder.ToTable("Opportunities");

              builder.HasKey(o => o.Id);

              builder.Property(o => o.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(o => o.LeadId)
                     .IsRequired();

              builder.Property(o => o.ProductId);

              builder.Property(o => o.UserId)
                     .IsRequired();

              builder.Property(o => o.Stage)
                     .IsRequired()
                     .HasConversion<int>();

              builder.Property(o => o.Amount)
                     .HasPrecision(18, 2);

              builder.Property(o => o.ExpectedCloseDate);

              builder.Property(o => o.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();

              builder.Property(o => o.SortOrder)
                     .IsRequired()
                     .HasDefaultValue(0);

              builder.HasOne(o => o.Lead)
                     .WithMany()
                     .HasForeignKey(o => o.LeadId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(o => o.User)
                     .WithMany()
                     .HasForeignKey(o => o.UserId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(o => o.Product)
                     .WithMany()
                     .HasForeignKey(o => o.ProductId)
                     .IsRequired(false)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}
