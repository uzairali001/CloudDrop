using System.Security.Claims;

namespace CloudDrop.Api.Core.Constants;
public class ClaimsConstant
{
    public const string Id = ClaimTypes.NameIdentifier;
    public const string Name = ClaimTypes.Name;
    public const string Email = ClaimTypes.Email;
    public const string Jti = "jti";
    public const string Role = ClaimTypes.Role;
}
