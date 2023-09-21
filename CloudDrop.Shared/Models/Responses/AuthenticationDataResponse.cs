namespace CloudDrop.Shared.Models.Responses;
public class AuthenticationDataResponse
{
    public required AccessTokenResponse AccessToken { get; set; }
    public required UserResponse User { get; set; }
}
