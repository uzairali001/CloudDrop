using AutoMapper;

using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.Data;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Shared.Models.Commands;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.Extensions.Configuration;

using MimeTypes;

namespace CloudDrop.Api.Core.Services.Data;
public class UploadSessionService : BaseService, IUploadSessionService
{
    private readonly IConfiguration _configuration;
    private readonly IUploadSessionRepository _uploadSessionRepository;
    private readonly IFileRepository _fileRepository;

    public UploadSessionService(IMapper mapper,
        IConfiguration configuration,
        IUploadSessionRepository uploadSessionRepository,
        IFileRepository fileRepository) : base(mapper)
    {
        _configuration = configuration;
        _uploadSessionRepository = uploadSessionRepository;
        _fileRepository=fileRepository;
    }

    public async Task<UploadSessionResponse?> CreateUploadSessionAsync(CreateUploadSessionCommand command, CancellationToken cancellation)
    {
        Uri publicUrl = new(_configuration.GetRequiredSection("PublicUrl").Value!);
        TimeSpan expiry = _configuration.GetValue<TimeSpan>("UploadSessionExpiry");

        string sessionId = Guid.NewGuid().ToString("N");
        DateTime expires = DateTime.UtcNow.Add(expiry);

        UploadSessionEntity entity = new()
        {
            UserId = command.UserId,
            SessionId = sessionId,
            FileName = Path.GetFileName(command.Name) ?? sessionId,
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
                UpdateUrl = new Uri(publicUrl, $"/upload/{sessionId}"),
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
            DateTime utcNow = DateTime.UtcNow;

            UploadSessionEntity uploadSession = await _uploadSessionRepository
                .GetFirstAsync(e => e.SessionId == request.SessionId, asTracking: true, cancellation: cancellation)
                ?? throw new Exception("Session not found");

            if (DateTime.UtcNow > uploadSession.ExpirationDateTime)
            {
                throw new Exception("Session expired, create new session and start over");
            }

            string uploadDirectory = _configuration.GetValue<string>("UploadPath")
                ?? throw new Exception("UploadPath not found in settings");

            Directory.CreateDirectory(uploadDirectory);

            string directoryPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, uploadDirectory));
            string filePath = Path.ChangeExtension(Path.Combine(directoryPath, request.SessionId), "dat");

            using (var stream = new FileStream(filePath, FileMode.Append))
            {
                await request.File.CopyToAsync(stream, cancellation);
            }

            bool isCompleted = request.BytesTo+1 == request.Size;

            TimeSpan expiry = _configuration.GetValue<TimeSpan>("UploadSessionExpiry");

            uploadSession.ExpirationDateTime = DateTime.UtcNow.Add(expiry);
            uploadSession.ReceivedBytes = request.BytesTo;
            uploadSession.Size = request.Size;

            if (request.BytesFrom == 0)
            {
                uploadSession.FirstByteReceivedAt = utcNow;
            }
            if (isCompleted)
            {
                uploadSession.CompletedAt = utcNow;
                uploadSession.ExpirationDateTime = utcNow;
            }

            bool isSaved = await _uploadSessionRepository.SaveChangesAsync(cancellation) > 0;
            if (!isSaved)
            {
                return false;
            }


            if (isCompleted)
            {
                // All bytes are uploaded
                FileInfo fileInfo = new(filePath);
                fileInfo.MoveTo(Path.Combine(directoryPath, uploadSession.FileName), true);

                await _fileRepository.AddOrUpdateAndSaveAsync(new FileEntity()
                {
                    UploadSessionId = uploadSession.Id,
                    UserId = uploadSession.UserId,
                    Name = uploadSession.FileName,
                    Size = uploadSession.Size,
                    MimeType = MimeTypeMap.GetMimeType(Path.GetExtension(uploadSession.FileName))
                }, cancellation);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
