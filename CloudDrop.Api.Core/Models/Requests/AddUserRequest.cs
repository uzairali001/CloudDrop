namespace CloudDrop.Api.Core.Models.Requests;
public record AddUserRequest(string FirstName, string? LastName, string? Username, string Email, string Password, bool? IsActive);