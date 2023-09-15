namespace CloudDrop.App.Core.Contracts.Services.Data;
public interface IMediaFileService
{
    Task AddAsync(string filePath);
    Task UploadAllFilesAsync(CancellationToken ct = default);
}
