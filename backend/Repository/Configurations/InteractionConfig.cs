using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

/// <summary>
/// Configuração de mapeamento da entidade <see cref="Interaction"/> para o Entity Framework Core.
/// </summary>
public class InteractionConfig : IEntityTypeConfiguration<Interaction>
{
       /// <summary>
       /// Define a estrutura da tabela, colunas, índices e relacionamentos de <see cref="Interaction"/>.
       /// </summary>
       /// <param name="builder">Builder utilizado pelo EF Core para configurar a entidade.</param>
    public void Configure(EntityTypeBuilder<Interaction> builder)
    {
        builder.ToTable("Interactions");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.CrmEntityId)
               .IsRequired();

        builder.Property(i => i.CrmEntityType)
               .IsRequired()
               .HasConversion<int>();

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

        builder.HasIndex(i => new { i.CrmEntityType, i.CrmEntityId });

        builder.HasOne(i => i.User)
               .WithMany(u => u.Interactions)
               .HasForeignKey(i => i.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}