using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Contracts.Services.Data;
public interface IUploadSessionService
{
    Task<UploadSessionResponse?> CreateUploadSessionAsync(CreateUploadSessionRequest req, CancellationToken cancellation = default);
    Task<bool> DestroySessionAsync(string sessionId, CancellationToken cancellation = default);
    Task<bool> UpdateUploadSessionAsync(UpdateUploadSessionRequest request, CancellationToken cancellation = default);
}
