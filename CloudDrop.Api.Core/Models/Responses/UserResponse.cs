namespace CloudDrop.Api.Core.Models.Responses;
public class UserResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public string? Username { get; set; }
}
