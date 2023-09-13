using CloudDrop.Api.Core.Enums;

namespace CloudDrop.Api.Core.Models.Requests;
public class CreateUploadSessionRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public ConflictBehaviors? ConflictBehavior { get; init; }
}
