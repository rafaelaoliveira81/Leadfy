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
    public DbSet<Ower> Owers { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new OwerConfig());
        modelBuilder.ApplyConfiguration(new LeadConfig());
        modelBuilder.ApplyConfiguration(new ProductConfig());

        base.OnModelCreating(modelBuilder);
    }
}
