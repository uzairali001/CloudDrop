namespace CloudDrop.App.Core.Entities;
public class AuthenticationEntity : Base.BaseEntity
{
    public string BaseUrl { get; set; } = null!;
    public string AuthToken { get; set; } = null!;
}
