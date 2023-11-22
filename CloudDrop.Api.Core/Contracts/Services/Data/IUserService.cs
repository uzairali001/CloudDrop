using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Shared.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Services.Data;
public interface IUserService
{
    Task<bool> AddUserAsync(AddUserRequest request, CancellationToken cancellation = default);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(IEnumerable<string> roles, uint? userId, CancellationToken cancellation = default);
    Task<UserEditResponse?> GetUserByIdAsync(uint id, CancellationToken cancellation = default);
    Task<bool> UpdateUserAsync(uint id, UpdateUserRequest request, CancellationToken cancellation = default);
}
