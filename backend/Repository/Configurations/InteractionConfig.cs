using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class InteractionConfig : IEntityTypeConfiguration<Interaction>
{
       public void Configure(EntityTypeBuilder<Interaction> builder)
       {
              builder.ToTable("Interactions");

              builder.HasKey(i => i.Id);

              builder.Property(i => i.OpportunityId)
                     .IsRequired();

              builder.Property(i => i.FromStage)
                     .IsRequired(false);

              builder.Property(i => i.ToStage)
                     .IsRequired(false);

              builder.Property(i => i.Description)
                     .IsRequired()
                     .HasMaxLength(1000);

              builder.Property(i => i.InteractionDate)
                     .IsRequired();

              builder.Property(i => i.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();

              builder.Property(i => i.NextContactDate)
                     .IsRequired(false);

              builder.Property(i => i.UserId)
                     .IsRequired();

              builder.HasOne(i => i.User)
                     .WithMany(u => u.Interactions)
                     .HasForeignKey(i => i.UserId)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}