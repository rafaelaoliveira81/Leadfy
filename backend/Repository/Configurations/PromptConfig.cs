using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class PromptConfig : IEntityTypeConfiguration<Prompt>
{
       public void Configure(EntityTypeBuilder<Prompt> builder)
       {
              builder.ToTable("Prompts");

              builder.HasKey(p => p.Id);

              builder.Property(p => p.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(p => p.Title)
                     .IsRequired()
                     .HasMaxLength(150);

              builder.Property(p => p.Content)
                     .HasMaxLength(1000);

              builder.Property(p => p.TenantId)
                     .IsRequired();

              builder.HasIndex(p => p.TenantId);

              builder.Property(p => p.IsActive)
                     .IsRequired();

              builder.Property(p => p.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();

              builder.HasOne(p => p.Tenant)
                     .WithMany(t => t.Prompts)
                     .HasForeignKey(p => p.TenantId)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}
