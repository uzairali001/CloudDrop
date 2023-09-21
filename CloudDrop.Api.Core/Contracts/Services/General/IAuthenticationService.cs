using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Services.General;
public interface IAuthenticationService
{
    Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest req, CancellationToken cancellation = default);
    Task<UserResponse> RegisterAsync(SaveUserRequest req, CancellationToken cancellation = default);
}
