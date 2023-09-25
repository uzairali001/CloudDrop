using AutoMapper;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Services.Data;
public class FileService(IMapper mapper, IFileRepository fileRepository) : BaseService(mapper), IFileService
{
    public async Task<IEnumerable<FileResponse>> GetRecordingsForUser(uint userId, CancellationToken cancellation = default)
    {
        return Enumerable.Empty<FileResponse>();
    }
}
