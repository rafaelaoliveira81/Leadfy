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

              builder.Property(p => p.IsActive)
                     .IsRequired();

              builder.Property(p => p.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
       }
}
