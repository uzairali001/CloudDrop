using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;

[Table("file")]
[Index(nameof(UserId), nameof(Name), Name = "UK_file_user_id_name", IsUnique = true)]
public class FileEntity : Base.BaseEntity
{
    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public required string Name { get; set; }

    [Column("mime_type")]
    [StringLength(255)]
    public required string MimeType { get; set; }

    [Column("size")]
    public long Size { get; set; }


    // Navigation Properties

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(UserEntity.Files))]
    public UserEntity User { get; set; } = null!;
}