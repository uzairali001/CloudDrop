namespace CloudDrop.App.Core.Models.Requests;
public class SigninRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
