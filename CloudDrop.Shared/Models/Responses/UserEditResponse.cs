namespace CloudDrop.Shared.Models.Responses;
public class UserEditResponse
{
    public uint Id { get; set; }
    public required uint TypeId { get; set; }
    public string? Username { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
