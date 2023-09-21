using CloudDrop.Shared.Enums;

namespace CloudDrop.Shared.Models.Commands;
public class CreateUploadSessionCommand
{
    public uint UserId { get; set; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public ConflictBehaviors? ConflictBehavior { get; init; }
}
