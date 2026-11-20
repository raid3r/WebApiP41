using Microsoft.EntityFrameworkCore;

namespace WebApiP41.Models;

public class ApiDbContext: DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<ImageFile> Images { get; set; }
    public virtual DbSet<SiteConfig> SiteConfig { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Product>()
    //        .Property(p => p.Price)
    //        .HasConversion<double>()
    //        .HasColumnType("REAL")
    //        ; // для SQLite
    //}

}
