namespace CloudDrop.App.Core.Models.Dtos;
public class AuthenticationDto
{
    public required string Username { get; set; }
    public required string ApiUrl { get; set; }
    public required string AuthToken { get; set; }
    public string? FilesDirectory { get; set; }
}
