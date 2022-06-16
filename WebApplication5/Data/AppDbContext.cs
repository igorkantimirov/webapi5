using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Data;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=database.db");
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(x =>
        {
            x.Property(x => x.Id).ValueGeneratedNever();
            x.Property(x => x.Username).IsRequired().HasMaxLength(255);
            x.Property(x => x.Email).IsRequired().HasMaxLength(320);
            x.Property(x => x.CreatedAt).IsRequired();
            x.Property(x => x.PasswordHashed).IsRequired();
            x.Property(x => x.LastLoginAt);
            x.Property(x => x.UpdatedAt);
        });
    }
}