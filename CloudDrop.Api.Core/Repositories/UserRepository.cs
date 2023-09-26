using BCrypt.Net;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace CloudDrop.Api.Core.Repositories;
public class UserRepository(CloudDropDbContext dbContext) : Base.BaseRepository<UserEntity>(dbContext), IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellation = default)
    {
        await AddAsync(user, cancellation);
        return await SaveChangesAsync(cancellation) > 0;
    }

    public async Task<UserEntity?> AuthenticateUserAsync(string username, string password, CancellationToken cancellation = default)
    {
        var user = await _entity
            .Include(e => e.UserRoles)
            .ThenInclude(e => e.Role)
            .Where(e => e.IsActive == true)
            .Where(e => e.IsDeleted == false)
            .FirstOrDefaultAsync(e => e.Username == username || e.Email == username);

        if (user is null)
        {
            return default;
        }

        return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : default;
    }

    public async Task<UserEntity?> GetByUsernameOrEmailAsync(string username, CancellationToken cancellation = default)
    {
        return await GetFirstAsync(e => e.Email == username || e.Username == username, cancellation: cancellation);
    }

    public async Task<UserEntity?> GetUserByIdAsync(uint userId, CancellationToken cancellation = default)
    {
        return await _entity
            .Include(e => e.UserRoles)
            .ThenInclude(e => e.Role)
            .Where(e => e.IsActive == true)
            .Where(e => e.IsDeleted == false)
            .FirstOrDefaultAsync(cancellation);
    }
}
