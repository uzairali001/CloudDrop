using CloudDrop.App.Core.Constants;

using Microsoft.EntityFrameworkCore;

namespace CloudDrop.App.Core.Entities;
public class CloudDropDbContext : DbContext
{
    public DbSet<AuthenticationEntity> Authentications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DatabaseConstants.DatabasePath)!);
        options.UseSqlite($"Data Source={DatabaseConstants.DatabasePath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}
