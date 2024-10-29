using Microsoft.EntityFrameworkCore;
using NioZero.Data.Entities;

namespace NioZero.Data;

public class WebDbContext(DbContextOptions<WebDbContext> options) : DbContext(options)
{
    public DbSet<BlogEntry> BlogEntries { get; set; }

    public DbSet<GlobalParameter> GlobalParameters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //
        // BlogEntries
        //
        modelBuilder.Entity<BlogEntry>()
            .HasIndex(u => u.Code)
            .IsUnique();

        modelBuilder.Entity<BlogEntry>()
            .ToTable(nameof(BlogEntries));


        //
        // GlobalParameters
        //
        modelBuilder.Entity<GlobalParameter>()
            .HasIndex(u => u.Key)
            .IsUnique();

        modelBuilder.Entity<GlobalParameter>()
            .ToTable(nameof(GlobalParameters));
    }
}
