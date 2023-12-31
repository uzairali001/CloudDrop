﻿using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Commands;

using Microsoft.EntityFrameworkCore;

namespace CloudDrop.Api.Core.Repositories;
public class FileRepository(CloudDropDbContext dbContext) : Base.BaseRepository<FileEntity>(dbContext), IFileRepository
{
    public async Task<bool> AddOrUpdateAndSaveAsync(FileEntity fileEntity, CancellationToken cancellation = default)
    {
        FileEntity? entity = await _entity
            .Where(e => e.UserId == fileEntity.UserId)
            .Where(e => e.Name == fileEntity.Name)
            .FirstOrDefaultAsync(cancellation);

        if (entity is not null)
        {
            entity.UploadSessionId = fileEntity.UploadSessionId;
            entity.Size = fileEntity.Size;

            return await UpdateAndSaveAsync(entity, cancellation);
        }

        return await AddAndSaveAsync(fileEntity, cancellation);
    }

    public Task<FileEntity?> GetFileForUserAsync(GetFileCommand command, CancellationToken cancellation = default)
    {
        return _entity.Where(e => e.Id == command.FileId)
            .Where(e => e.UserId == command.UserId)
            .FirstOrDefaultAsync(cancellation);
    }
}
