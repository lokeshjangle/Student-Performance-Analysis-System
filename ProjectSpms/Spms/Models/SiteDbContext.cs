using Microsoft.EntityFrameworkCore;

namespace spms.Models;

  public class SiteDbContext : DbContext
  {

    public SiteDbContext(DbContextOptions options) : base(options){}

    public DbSet<Admin> Admins { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("Data Source=192.168.255.49;Database=spms;User Id=root;Password=120801");
    }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //   base.OnModelCreating(modelBuilder);

    //   modelBuilder.Entity<Publisher>(entity =>
    //   {
    //     entity.HasKey(e => e.ID);
    //     entity.Property(e => e.Name).IsRequired();
    //   });

    //   modelBuilder.Entity<Book>(entity =>
    //   {
    //     entity.HasKey(e => e.ISBN);
    //     entity.Property(e => e.Title).IsRequired();
    //     entity.HasOne(d => d.Publisher)
    //       .WithMany(p => p.Books);
    //   });
    // }
  }
