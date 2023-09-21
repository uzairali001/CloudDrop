﻿using CloudDrop.Api.Core.Entities.Base;

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CloudDrop.Api.Core.Entities;

[Table("user")]
[Index(nameof(Email), Name = "UK_user_email_address", IsUnique = true)]
[Index(nameof(Username), Name = "UK_user_user_name", IsUnique = true)]
public class UserEntity : BaseEntity
{
    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Column("user_name")]
    [StringLength(100)]
    public string? Username { get; set; }

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;


    // Navigation Properties
    [InverseProperty(nameof(FileEntity.User))]
    public ICollection<FileEntity> Files { get; set; } = new HashSet<FileEntity>();

    [InverseProperty(nameof(UploadSessionEntity.User))]
    public ICollection<UploadSessionEntity> UploadSessions { get; set; } = new HashSet<UploadSessionEntity>();
}