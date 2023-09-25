using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Services.Data;
public interface IFileService
{
    Task<IEnumerable<FileResponse>> GetRecordingsForUser(uint userId, CancellationToken cancellation = default);
}
