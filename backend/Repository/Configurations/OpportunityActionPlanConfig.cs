using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class OpportunityActionPlanConfig : IEntityTypeConfiguration<OpportunityActionPlan>
{
       public void Configure(EntityTypeBuilder<OpportunityActionPlan> builder)
       {
              builder.ToTable("OpportunityActionPlans");

              builder.HasKey(ap => ap.Id);

              builder.Property(p => p.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(ap => ap.TenantId)
                     .IsRequired();

              builder.Property(ap => ap.Message)
                     .HasColumnType("nvarchar(max)");

              builder.Property(ap => ap.ActionPlan)
                     .IsRequired()
                     .HasColumnType("nvarchar(max)");

              builder.Property(ap => ap.GeneratedAt)
                     .IsRequired();

              builder.Property(ap => ap.OpportunityId)
                     .IsRequired();

              builder.HasIndex(ap => ap.TenantId);

              builder.HasOne(ap => ap.Tenant)
                     .WithMany(t => t.OpportunityActionPlans)
                     .HasForeignKey(ap => ap.TenantId)
                     .OnDelete(DeleteBehavior.Restrict);

              builder.HasOne(ap => ap.Opportunity)
                     .WithMany(o => o.ActionPlans)
                     .HasForeignKey(ap => ap.OpportunityId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}
