namespace CloudDrop.Api.Core.Models.Responses;
public class AuthenticationResponse
{
    public bool IsAuthenticated { get; set; }
    public required string Message { get; set; }
    public AuthenticationDataResponse? Data { get; set; }
}
