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

    public async Task<AuthenticationDto?> GetAsync(CancellationToken cancellation = default)
    {
        var entity = await _authenticationRepository.GetFirstOrDefaultAsync(tracking: false, cancellation);
        if (entity is null)
        {
            return default;
        }

        return new AuthenticationDto()
        {
            AuthToken = entity.AuthToken,
            ApiUrl = entity.ApiUrl,
            FilesDirectory = entity.FilesDirectory,
            Username = entity.Username,
        };
    }

    public async Task<bool> AddOrUpdateAsync(AuthenticationDto authenticationDto, CancellationToken cancellation = default)
    {
        AuthenticationEntity? entity = await _authenticationRepository.GetFirstOrDefaultAsync(tracking: true, cancellation);
        if (entity is not null)
        {
            entity.FilesDirectory = authenticationDto.FilesDirectory;
            entity.AuthToken = authenticationDto.AuthToken;
            entity.ApiUrl = authenticationDto.ApiUrl;
            entity.Username = authenticationDto.Username;

            return await _authenticationRepository.SaveChangesAsync(cancellation) > 0;
        }

        AuthenticationEntity newEntity = new()
        {
            FilesDirectory = authenticationDto.FilesDirectory,
            AuthToken = authenticationDto.AuthToken,
            ApiUrl = authenticationDto.ApiUrl,
            Username = authenticationDto.Username,
        };

        await _authenticationRepository.InsertAsync(newEntity);
        return await _authenticationRepository.SaveChangesAsync(cancellation) > 0;
    }
}
