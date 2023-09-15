using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Models.Dtos;
using CloudDrop.App.Core.Services.Data.Base;

namespace CloudDrop.App.Core.Services.Data;
public class AuthenticationService : BaseService, IAuthenticationService
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public AuthenticationService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<string?> GetAuthenticationTokenAsync(CancellationToken ct = default)
    {
        return (await _authenticationRepository.GetFirstOrDefaultAsync(tracking: false, ct))?.AuthToken;
    }

    public async Task<string?> GetBaseUrlAsync(CancellationToken ct = default)
    {
        return (await _authenticationRepository.GetFirstOrDefaultAsync(tracking: false, ct))?.BaseUrl;
    }

    public async Task<bool> AddOrUpdateAuthenticationTokenAsync(string authToken, CancellationToken ct = default)
    {
        var entity = await _authenticationRepository.GetFirstOrDefaultAsync(tracking: true, ct);
        if (entity is not null)
        {
            entity.AuthToken = authToken;
            return await _authenticationRepository.SaveChangesAsync(ct) > 0;
        }

        await _authenticationRepository.InsertAsync(new AuthenticationEntity()
        {
            AuthToken = authToken,
            BaseUrl = "",
        });
        return await _authenticationRepository.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> AddOrUpdateBaseUrlAsync(string baseUrl, CancellationToken ct = default)
    {
        var entity = await _authenticationRepository.GetFirstOrDefaultAsync(tracking: true, ct);
        if (entity is not null)
        {
            entity.BaseUrl = baseUrl;
            return await _authenticationRepository.SaveChangesAsync(ct) > 0;
        }

        await _authenticationRepository.InsertAsync(new AuthenticationEntity()
        {
            AuthToken = "",
            BaseUrl = baseUrl,
        });
        return await _authenticationRepository.SaveChangesAsync(ct) > 0;
    }

    public Task<AuthenticationDto?> GetAsync(CancellationToken cancellation = default)
    {
        var entity = await _authenticationRepository.GetFirstOrDefaultAsync(tracking: false, ct)
    }
}
