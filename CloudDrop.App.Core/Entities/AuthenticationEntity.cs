namespace CloudDrop.App.Core.Entities;
public class AuthenticationEntity : Base.BaseEntity
{
    public string Username { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
    public string AuthToken { get; set; } = null!;
    public string? FilesDirectory { get; set; }
}
