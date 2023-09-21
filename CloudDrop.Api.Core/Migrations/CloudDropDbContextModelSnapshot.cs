﻿// <auto-generated />
using System;
using CloudDrop.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CloudDrop.Api.Core.Migrations
{
    [DbContext(typeof(CloudDropDbContext))]
    partial class CloudDropDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.FileEntity", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("mime_type");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<uint?>("UploadSessionId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("upload_session_id");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("UploadSessionId")
                        .IsUnique();

                    b.HasIndex(new[] { "UserId", "Name" }, "UK_file_user_id_name")
                        .IsUnique();

                    b.ToTable("file");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.UploadSessionEntity", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("completed_at");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpirationDateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("expiration_datetime");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("file_name");

                    b.Property<DateTime?>("FirstByteReceivedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("first_byte_received_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<long>("ReceivedBytes")
                        .HasColumnType("bigint")
                        .HasColumnName("received_bytes");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("session_id");

                    b.Property<long>("Size")
                        .HasColumnType("bigint")
                        .HasColumnName("size");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<uint>("UserId")
                        .HasColumnType("int unsigned")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "SessionId" }, "UK_upload_session_session_id")
                        .IsUnique();

                    b.ToTable("upload_session");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.UserEntity", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_deleted");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Email" }, "UK_user_email_address")
                        .IsUnique();

                    b.HasIndex(new[] { "Username" }, "UK_user_user_name")
                        .IsUnique();

                    b.ToTable("user");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.FileEntity", b =>
                {
                    b.HasOne("CloudDrop.Api.Core.Entities.UploadSessionEntity", "Session")
                        .WithOne("File")
                        .HasForeignKey("CloudDrop.Api.Core.Entities.FileEntity", "UploadSessionId")
                        .HasConstraintName("FK_file_upload_session_id");

                    b.HasOne("CloudDrop.Api.Core.Entities.UserEntity", "User")
                        .WithMany("Files")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_file_user_id");

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.UploadSessionEntity", b =>
                {
                    b.HasOne("CloudDrop.Api.Core.Entities.UserEntity", "User")
                        .WithMany("UploadSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_upload_session_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.UploadSessionEntity", b =>
                {
                    b.Navigation("File");
                });

            modelBuilder.Entity("CloudDrop.Api.Core.Entities.UserEntity", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("UploadSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
