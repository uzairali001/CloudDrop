using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Contracts.Repositories;
public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellation = default);
    Task<UserEntity?> AuthenticateUserAsync(string username, string password, CancellationToken cancellation = default);
    Task<UserEntity?> GetByUsernameOrEmailAsync(string username, CancellationToken cancellation = default);
    Task<UserEntity?> GetUserByIdAsync(uint userId, CancellationToken cancellation = default);
    Task<IEnumerable<UserEntity>> GetByRoleAsync(IEnumerable<string> roles, uint? userId, CancellationToken cancellation = default);
}
