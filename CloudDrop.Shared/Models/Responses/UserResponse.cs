namespace CloudDrop.Shared.Models.Responses;
public class UserResponse
{
    public uint Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
