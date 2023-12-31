﻿using CloudDrop.Shared.Enums;

namespace CloudDrop.Shared.Models.Requests;
public class CreateUploadSessionRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public ConflictBehaviors? ConflictBehavior { get; init; }
}
