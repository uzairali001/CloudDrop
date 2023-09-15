using CloudDrop.App.Core.Entities.Base;

namespace CloudDrop.App.Core.Entities;
public class MediaFileEntity : BaseEntity
{
    public string Filename { get; set; } = null!;
}
