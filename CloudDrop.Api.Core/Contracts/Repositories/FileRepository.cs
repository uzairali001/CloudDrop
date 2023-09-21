using CloudDrop.Api.Core.Entities;

namespace CloudDrop.Api.Core.Contracts.Repositories;
public interface IFileRepository : IBaseRepository<FileEntity>
{
    Task<bool> AddOrUpdateAndSaveAsync(FileEntity fileEntity, CancellationToken cancellation = default);
}
