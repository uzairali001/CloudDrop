using AutoMapper;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.Extensions.Configuration;

namespace CloudDrop.Api.Core.Services.Data;
public class UploadSessionService : BaseService, IUploadSessionService
{
    private readonly IConfiguration _configuration;
    private readonly IUploadSessionRepository _uploadSessionRepository;

    public UploadSessionService(IMapper mapper,
        IConfiguration configuration,
        IUploadSessionRepository uploadSessionRepository) : base(mapper)
    {
        _configuration = configuration;
        _uploadSessionRepository = uploadSessionRepository;
    }

    public async Task<UploadSessionResponse?> CreateUploadSessionAsync(CreateUploadSessionRequest req, CancellationToken cancellation)
    {
        Uri baseUri = new(_configuration.GetRequiredSection("AppUrl").Value!);
        TimeSpan expiry = _configuration.GetValue<TimeSpan>("UploadSessionExpiry");

        string sessionId = Guid.NewGuid().ToString("N");
        DateTime expires = DateTime.UtcNow.Add(expiry);

        UploadSessionEntity entity = new()
        {
            UserId = req.UserId,
            SessionId = sessionId,
            FileName = Path.GetFileName(req.Name) ?? sessionId,
            ExpirationDateTime = expires,
            ReceivedBytes = 0,
            Size = 0,
        };

        if (await _uploadSessionRepository.AddAndSaveAsync(entity, cancellation))
        {
            return new UploadSessionResponse()
            {
                SessionId = sessionId,
                ExpirationDateTime = expires,
                UpdateUrl = new Uri(baseUri, $"/upload/{sessionId}"),
            };
        }

        return default;
    }

    public async Task<bool> DestroySessionAsync(string sessionId, CancellationToken cancellation = default)
    {
        var entity = await _uploadSessionRepository.GetFirstAsync(e => e.SessionId == sessionId, asTracking: true, cancellation);

        if (entity is null || DateTime.UtcNow > entity.ExpirationDateTime)
        {
            return false;
        }

        entity.IsDeleted = true;
        return await _uploadSessionRepository.UpdateAndSaveAsync(entity, cancellation);
    }

    public async Task<bool> UpdateUploadSessionAsync(UpdateUploadSessionRequest request, CancellationToken cancellation = default)
    {
        try
        {
            UploadSessionEntity uploadSession = await _uploadSessionRepository
                .GetFirstAsync(e => e.SessionId == request.SessionId, asTracking: true, cancellation: cancellation)
                ?? throw new Exception("Session not found");

            if (DateTime.UtcNow > uploadSession.ExpirationDateTime)
            {
                throw new Exception("Session expired, create new session and start over");
            }

            var a = uploadSession.ReceivedBytes;
            var b = request.ReceivedBytes;

            string uploadDirectory = _configuration.GetValue<string>("UploadPath")
                ?? throw new Exception("UploadPath not found in settings");

            Directory.CreateDirectory(uploadDirectory);

            string directoryPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, uploadDirectory));
            string filePath = Path.ChangeExtension(Path.Combine(directoryPath, request.SessionId), "dat");

            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                await request.File.CopyToAsync(stream, cancellation);
            }

            if (request.ReceivedBytes+1 == request.Size)
            {
                // All bytes are uploaded
                FileInfo fileInfo = new(filePath);
                fileInfo.MoveTo(Path.Combine(directoryPath, uploadSession.FileName), true);
            }

            TimeSpan expiry = _configuration.GetValue<TimeSpan>("UploadSessionExpiry");

            uploadSession.ExpirationDateTime = DateTime.UtcNow.Add(expiry);
            uploadSession.ReceivedBytes = request.ReceivedBytes;
            uploadSession.Size = request.Size;

            await Task.Delay(1000);

            return await _uploadSessionRepository.SaveChangesAsync() > 0;

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
