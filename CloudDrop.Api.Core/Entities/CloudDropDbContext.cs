using Microsoft.EntityFrameworkCore;

namespace CloudDrop.Api.Core.Entities;
public class CloudDropDbContext : DbContext
{
    public DbSet<UploadSessionEntity> UploadSessions { get; set; }



    public CloudDropDbContext(DbContextOptions<CloudDropDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UploadSessionEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}
