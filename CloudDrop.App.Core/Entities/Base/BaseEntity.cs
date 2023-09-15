using System.ComponentModel.DataAnnotations;

namespace CloudDrop.App.Core.Entities.Base;
public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
