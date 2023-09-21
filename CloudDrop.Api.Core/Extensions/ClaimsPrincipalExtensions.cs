using System.Security.Claims;

namespace CloudDrop.Api.Core.Extensions;
public static class ClaimsPrincipalExtensions
{
    public static TValue? FindFirstValue<TValue>(this ClaimsPrincipal principal, string name)
    {
        Claim? claim = principal.FindFirst(name);
        return claim is not null
            ? (TValue)Convert.ChangeType(claim.Value, typeof(TValue))
            : default;
    }
    public static TValue RequiredFirstValue<TValue>(this ClaimsPrincipal principal, string name)
    {
        Claim claim = principal.FindFirst(name)
            ?? throw new Exception($"{name} Claim not found.");

        return (TValue)Convert.ChangeType(claim.Value, typeof(TValue));
    }
}
