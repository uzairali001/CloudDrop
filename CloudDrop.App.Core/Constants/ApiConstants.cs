namespace CloudDrop.App.Core.Constants;
public static class ApiConstants
{
    public const string AuthRouteEndpoint = "api/auth/";
    public const string SigninEndpoint = AuthRouteEndpoint + "signin";
    public const string AuthTokenRefresh = AuthRouteEndpoint + "token";

    public const string MediaRouteEndpoint = "api/media/";
    public const string FileUploadEndpoint = MediaRouteEndpoint + "upload";
}
