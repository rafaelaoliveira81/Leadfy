using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Repository.Configurations;

namespace Repository.Context;

public class CRMContext : DbContext
{
    public CRMContext(DbContextOptions<CRMContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<OpportunityActionPlan> OpportunityActionPlans { get; set; }
    public DbSet<Interaction> Interactions { get; set; }
    public DbSet<AiConfig> AiConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());

        modelBuilder.ApplyConfiguration(new LeadConfig());
        modelBuilder.ApplyConfiguration(new ProductConfig());
        modelBuilder.ApplyConfiguration(new OpportunityConfig());
        modelBuilder.ApplyConfiguration(new OpportunityActionPlanConfig());
        modelBuilder.ApplyConfiguration(new InteractionConfig());
        modelBuilder.ApplyConfiguration(new AiConfigConfig());

        base.OnModelCreating(modelBuilder);
    }
}
