using CloudDrop.App.Core.Models.Dtos;

namespace CloudDrop.App.Core.Contracts.Services.Data;
public interface IAuthenticationService
{

    Task<string?> GetAuthenticationTokenAsync(CancellationToken ct = default);
    Task<bool> AddOrUpdateAuthenticationTokenAsync(string authToken, CancellationToken ct = default);

    Task<string?> GetBaseUrlAsync(CancellationToken ct = default);
    Task<bool> AddOrUpdateBaseUrlAsync(string baseUrl, CancellationToken ct = default);
    Task<AuthenticationDto?> GetAsync(CancellationToken cancellation = default);
}
