using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities;
public class UploadSessionEntity : Base.BaseEntity
{
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
}
