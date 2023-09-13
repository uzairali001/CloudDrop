namespace CloudDrop.Api.Core.Models.Requests;
public record AuthenticationRequest(string Email, string Password, bool? Remember = false);