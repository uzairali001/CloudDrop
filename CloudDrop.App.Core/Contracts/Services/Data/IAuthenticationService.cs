using CloudDrop.App.Core.Models.Dtos;

namespace CloudDrop.App.Core.Contracts.Services.Data;
public interface IAuthenticationService
{
    Task<AuthenticationDto?> GetAsync(CancellationToken cancellation = default);
    Task<bool> AddOrUpdateAsync(AuthenticationDto authenticationDto, CancellationToken cancellation = default);
}
