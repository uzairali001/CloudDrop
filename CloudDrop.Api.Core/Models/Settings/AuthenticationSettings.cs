namespace CloudDrop.Api.Core.Models.Settings;
public class AuthenticationSettings
{
    public const string SettingsKey = "Jwt";

    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public TimeSpan AccessTokenExpiry { get; set; }
    public TimeSpan RefreshTokenExpiry { get; set; }
}
