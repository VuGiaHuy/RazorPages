using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace GiaHuy.Models;
//GiaHuy.Models.GiaHuyDbContext
public class GiaHuyDbContext:IdentityDbContext<AppUser>
{
    public GiaHuyDbContext(DbContextOptions<GiaHuyDbContext> options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach(var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if(tableName.StartsWith("Asp"))
            {
                entity.SetTableName(tableName.Substring(6));
            }
        }
    }
    public DbSet<SinhVien> sinhVien {get;set;}
}