using Microsoft.EntityFrameworkCore;
namespace GiaHuy.Models;
public class GiaHuyDbContext:DbContext
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
    }
    public DbSet<SinhVien> sinhVien {get;set;}
}