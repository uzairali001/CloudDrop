using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Commands;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Repositories;
public interface IFileRepository : IBaseRepository<FileEntity>
{
    Task<bool> AddOrUpdateAndSaveAsync(FileEntity fileEntity, CancellationToken cancellation = default);
    Task<FileEntity?> GetFileForUserAsync(GetFileCommand command, CancellationToken cancellation = default);
}
