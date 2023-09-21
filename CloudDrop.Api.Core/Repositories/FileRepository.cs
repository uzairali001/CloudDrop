using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Repositories;
public class FileRepository(CloudDropDbContext dbContext) : Base.BaseRepository<FileEntity>(dbContext), IFileRepository
{

}
