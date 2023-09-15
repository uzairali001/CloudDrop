namespace CloudDrop.App.Core.Models.Responses;
public class SigninResponse
{
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public string? Username { get; set; }
    public string Email { get; set; } = null!;
    public string AuthToken { get; set; } = null!;
}
