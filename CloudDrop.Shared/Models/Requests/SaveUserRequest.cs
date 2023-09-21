namespace CloudDrop.Shared.Models.Requests;
public record SaveUserRequest(string FirstName, string LastName, string Email, string Password, string? Username);
