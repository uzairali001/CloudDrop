using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudDrop.Api.Core.Entities.Base;
public abstract class BaseEntity
{
    [Key]
    [Column("id", Order = 0)]
    public uint Id { get; set; }


    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; }
}
