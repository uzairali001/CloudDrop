using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Repositories.Base;

namespace CloudDrop.App.Core.Repositories;
public class MediaFileRepository : BaseRepository<MediaFileEntity>, IMediaFileRepository
{
    public MediaFileRepository(CloudDropDbContext dbContext) : base(dbContext)
    {
    }
}
