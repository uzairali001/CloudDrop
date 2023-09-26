using CloudDrop.Api.Core.Models.Commands;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Services.Data;
public interface IFileService
{
    Task<FileResponse?> GetRecordingFileAsync(GetFileCommand command, CancellationToken cancellation = default);
    Task<IEnumerable<FileResponse>> GetRecordingsForUserAsync(uint userId, CancellationToken cancellation = default);
}
