using Microsoft.EntityFrameworkCore;

namespace CloudDrop.Api.Core.Entities;
public class CloudDropDbContext(DbContextOptions<CloudDropDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<UploadSessionEntity> UploadSessions { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<FileEntity> Files { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UploadSessionEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User).WithMany(p => p.UploadSessions)
                .HasConstraintName("FK_upload_session_user_id");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<FileEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User).WithMany(x => x.Files)
                .HasConstraintName("FK_file_user_id");

            entity.HasOne(e => e.Session).WithOne(x => x.File)
               .HasConstraintName("FK_file_upload_session_id");
        });
    }
}
