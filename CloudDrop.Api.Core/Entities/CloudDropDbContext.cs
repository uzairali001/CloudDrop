using Microsoft.EntityFrameworkCore;

namespace CloudDrop.Api.Core.Entities;
public class CloudDropDbContext(DbContextOptions<CloudDropDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<UploadSessionEntity> UploadSessions { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserTypeEntity> UserTypes { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
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

            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

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

            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

        });

        modelBuilder.Entity<UserTypeEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

            entity.HasMany(e => e.Users)
                .WithOne(p => p.Type);
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
               .OnDelete(DeleteBehavior.ClientCascade)
               .HasConstraintName("FK_user_role_role_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
               .OnDelete(DeleteBehavior.ClientCascade)
               .HasConstraintName("FK_user_role_user_id");
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

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

            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'");

            entity.HasOne(e => e.User).WithMany(x => x.Files)
                .HasConstraintName("FK_file_user_id");

            entity.HasOne(e => e.Session).WithOne(x => x.File)
               .HasConstraintName("FK_file_upload_session_id");
        });
    }
}
