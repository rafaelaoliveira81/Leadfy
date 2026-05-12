using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Repository.Configurations;

public class AiConfigConfig : IEntityTypeConfiguration<AiConfig>
{
       public void Configure(EntityTypeBuilder<AiConfig> builder)
       {
              builder.ToTable("AiConfigs");

              builder.HasKey(c => c.Id);

              builder.Property(c => c.Title)
                     .IsRequired()
                     .HasMaxLength(150);

              builder.Property(c => c.PromptTemplate)
                     .IsRequired()
                     .HasMaxLength(2000);

              builder.Property(c => c.Model)
                     .IsRequired()
                     .HasMaxLength(100);

              builder.Property(c => c.ApiKeyHash)
                     .IsRequired()
                     .HasMaxLength(512);

              builder.Property(c => c.IsActive)
                     .IsRequired();

              builder.Property(c => c.CreatedAt)
                     .IsRequired()
                     .ValueGeneratedOnAdd();
       }
}
