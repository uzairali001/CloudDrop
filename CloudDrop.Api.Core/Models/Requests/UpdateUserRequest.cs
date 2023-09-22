namespace CloudDrop.Api.Core.Models.Requests;
public record UpdateUserRequest(string FirstName, string LastName, string? Username, string Email, string Password, bool IsActive);