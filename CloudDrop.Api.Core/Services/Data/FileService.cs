using AutoMapper;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Commands;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Services.Data;
public class FileService(IMapper mapper, IFileRepository fileRepository) : BaseService(mapper), IFileService
{
    public async Task<FileResponse?> GetRecordingFileAsync(GetFileCommand command, CancellationToken cancellation = default)
    {
        FileEntity? file = await fileRepository.GetFileForUserAsync(command, cancellation);
        if (file is null)
        {
            return default;
        }
        return _mapper.Map<FileResponse>(file);
    }

    public async Task<IEnumerable<FileResponse>> GetRecordingsForUserAsync(uint userId, CancellationToken cancellation = default)
    {
        var entities = await fileRepository.GetAsync(e => e.UserId == userId, cancellation: cancellation);
        
        return _mapper.Map<IEnumerable<FileResponse>>(entities)
            ?? Enumerable.Empty<FileResponse>();
    }
}
