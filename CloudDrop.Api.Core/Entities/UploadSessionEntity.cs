using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;

[Table("upload_session")]

[Index(nameof(SessionId), Name = "UK_upload_session_session_id", IsUnique = true)]
public class UploadSessionEntity : Base.BaseEntity
{
    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("session_id")]
    [StringLength(50)]
    public required string SessionId { get; set; }

    [Column("file_name")]
    [StringLength(100)]
    public required string FileName { get; set; }

    [Column("size")]
    public long Size { get; set; }

    [Column("received_bytes")]
    public long ReceivedBytes { get; set; }

    [Column("expiration_datetime")]
    public DateTime ExpirationDateTime { get; set; }

    [Column("first_byte_received_at")]
    public DateTime? FirstByteReceivedAt { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }


    // Navigation Properties
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(UserEntity.UploadSessions))]
    public UserEntity User { get; set; } = null!;

    [InverseProperty(nameof(FileEntity.Session))]
    public FileEntity? File { get; set; }
}
