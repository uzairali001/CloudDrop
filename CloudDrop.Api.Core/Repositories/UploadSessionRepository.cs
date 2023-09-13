using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Repositories;
public class UploadSessionRepository : Base.BaseRepository<UploadSessionEntity>, IUploadSessionRepository
{
    public UploadSessionRepository(CloudDropDbContext dbContext) : base(dbContext)
    {
    }
}
