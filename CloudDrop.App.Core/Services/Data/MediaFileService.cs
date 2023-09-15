using CloudDrop.App.Core.Constants;
using CloudDrop.App.Core.Contracts.Repositories;
using CloudDrop.App.Core.Contracts.Services.Data;
using CloudDrop.App.Core.Entities;
using CloudDrop.App.Core.Services.Data.Base;
using CloudDrop.App.Core.Services.General;

using Microsoft.Extensions.Logging;

using System.Net.Http.Headers;

namespace CloudDrop.App.Core.Services.Data;
public class MediaFileService : BaseService, IMediaFileService
{
    private readonly IMediaFileRepository _mediaFileRepository;
    private readonly AppAuthenticationService _appAuthenticationService;
    private readonly ILogger _logger;

    public MediaFileService(IMediaFileRepository mediaFileRepository,
        AppAuthenticationService appAuthenticationService,
        ILogger<MediaFileService> logger)
    {
        _mediaFileRepository = mediaFileRepository;
        _appAuthenticationService = appAuthenticationService;
        _logger = logger;
    }

    public async Task AddAsync(string filePath)
    {
        await _mediaFileRepository.InsertAsync(new MediaFileEntity()
        {
            Filename = filePath,
        });

        await _mediaFileRepository.SaveChangesAsync();
    }

    public async Task UploadAllFilesAsync(CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(_appAuthenticationService.AuthToken)
            || string.IsNullOrEmpty(_appAuthenticationService.BaseUrl))
        {
            _logger.LogInformation($"Skipping file Upload, Invalid Token: {_appAuthenticationService.AuthToken} or BaseUrl: {_appAuthenticationService.BaseUrl}");
            return;
        }

        var files = await _mediaFileRepository.GetAllAsync(ct);
        if (files is null || files.Any() is false)
        {
            return;
        }
        _logger.LogInformation($"Uploading {files.Count()} Files...");


        try
        {
            using var client = new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(20),
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appAuthenticationService.AuthToken);

            foreach (var file in files)
            {
                string filePath = Path.Combine(AppConstants.MediaDirectory, file.Filename);
                var fileContent = await GetMultipartFormDataContent(filePath, new Dictionary<string, string>()
                {
                    { "createdAt", file.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss") }
                });

                HttpResponseMessage response = await client.PostAsync(_appAuthenticationService.BaseUrl + ApiConstants.FileUploadEndpoint, fileContent, ct);
                fileContent.Dispose();

                if (response.IsSuccessStatusCode == false)
                {
                    // If status is unauthorized
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        //_authenticationService.GetRefreshAuthToken();
                    }
                    continue;
                }

                _logger.LogInformation($"File {file.Filename} uploaded.");
                // When API respond success
                await _mediaFileRepository.DeleteAsync(file);
                await _mediaFileRepository.SaveChangesAsync();

                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

    }

    private async Task<MultipartFormDataContent> GetMultipartFormDataContent(string filePath, IDictionary<string, string>? additionalData = default)
    {
        //Add the file
        var stream = File.OpenRead(filePath);

        MultipartFormDataContent multipartFormContent = new();

        //Add additional fields if any
        if (additionalData is not null)
        {
            foreach (var data in additionalData)
            {
                multipartFormContent.Add(new StringContent(data.Value), name: data.Key);
            }
        }
        multipartFormContent.Add(new StreamContent(stream), name: "file", fileName: Path.GetFileName(filePath));
        return multipartFormContent;
    }
}
