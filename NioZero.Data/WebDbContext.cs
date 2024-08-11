using Microsoft.EntityFrameworkCore;
using NioZero.Data.Entities;

namespace NioZero.Data;

public class WebDbContext(DbContextOptions<WebDbContext> options) : DbContext(options)
{
    public DbSet<BlogEntry> BlogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogEntry>()
            .ToTable(nameof(BlogEntries));
    }
}
