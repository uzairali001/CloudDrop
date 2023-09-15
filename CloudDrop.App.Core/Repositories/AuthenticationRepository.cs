using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Entities;

namespace CloudDrop.App.Core.Repositories;
public class AuthenticationRepository : Base.BaseRepository<AuthenticationEntity>, IAuthenticationRepository
{
    public AuthenticationRepository(CloudDropDbContext dbContext) : base(dbContext)
    {
    }
}
