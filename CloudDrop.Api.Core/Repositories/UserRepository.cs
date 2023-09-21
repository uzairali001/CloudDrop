using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Repositories;
public class UserRepository(CloudDropDbContext dbContext) : Base.BaseRepository<UserEntity>(dbContext), IUserRepository
{
    public async Task<bool> AddUserAsync(UserEntity user, CancellationToken cancellation = default)
    {
        await AddAsync(user, cancellation);
        return await SaveChangesAsync(cancellation) > 0;
    }

    public async Task<UserEntity?> GetByUsernameOrEmailAsync(string username, CancellationToken cancellation = default)
    {
        return await GetFirstAsync(e => e.Email == username || e.Username == username, cancellation: cancellation);
    }
}
