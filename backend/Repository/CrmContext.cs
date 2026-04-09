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
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
   
        base.OnModelCreating(modelBuilder);
    }
}
