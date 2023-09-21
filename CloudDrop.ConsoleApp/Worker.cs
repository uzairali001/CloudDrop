using CloudDrop.App.Core.Models.Dtos;
using CloudDrop.App.Core.Services.General;
using CloudDrop.Shared.Enums;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using System.Net.Http.Headers;

using UzairAli.NetHttpClient;

namespace CloudDrop.ConsoleApp;

public class Worker : PeriodicBackgroundService
{
    private readonly int _chunkSize = 320 * 1024 * 4; // 1.25 MiB
    private readonly AuthenticationDto _session;

    private readonly IHttpClientService _httpClient;

    public Worker(ILogger<Worker> logger,
        IHttpClientService httpClient,
        AppSessionService appSessionService) : base(logger)
    {
        _period = TimeSpan.FromMinutes(10);

        _httpClient = httpClient;

        if (appSessionService.Session is null)
        {
            throw new Exception("Invalid session");
        }
        if (string.IsNullOrEmpty(appSessionService.Session.FilesDirectory))
        {
            throw new Exception("Files directory is not set");
        }

        _session = appSessionService.Session;


        _ = OnTickAsync(new CancellationToken());
    }


    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        try
        {
            string[]? files = Directory.GetFiles(_session.FilesDirectory!, "*.mp3");
            if (files is null || files.Length == 0)
            {
                return;
            }

            _logger.LogInformation($"Uploading {files.Length} files");

            foreach (var file in files)
            {
                // Get Upload session
                UploadSessionResponse uploadSession = await GetUploadSession(file, stoppingToken);

                // Upload file as chunks
                await UploadAsChunksAsync(file, uploadSession, stoppingToken);
                
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Uploading failed");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

    }

    private async Task UploadAsChunksAsync(string filePath, UploadSessionResponse uploadSession, CancellationToken stoppingToken = default)
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri(_session.ApiUrl),
        };
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _session.AuthToken);

        long startByte = 0;
        long fileSize = new FileInfo(filePath).Length;
        var chunks = FileChunkService.ChunkAsync(filePath, _chunkSize, cancellationToken: stoppingToken);

        _logger.LogInformation($"----------------- {Path.GetFileName(filePath)} -----------------");

        await foreach (var item in chunks)
        {
            ContentRangeHeaderValue contentRange = new(startByte, startByte + item.Length -1, fileSize);

            _logger.LogInformation($"Uploading bytes: {contentRange}");

            using (MultipartFormDataContent form = new())
            {
                var byteContent = new ByteArrayContent(item.ToArray());

                form.Add(byteContent, "chunk", Path.GetFileName(filePath));
                form.Headers.ContentRange = contentRange;

                await RetryService.ExecuteAsync(async () =>
                {
                    var response = await httpClient.PutAsync($"upload/{uploadSession.SessionId}", form, stoppingToken);
                    response.EnsureSuccessStatusCode();
                });
            }

            startByte += item.Length;
        }
    }

    private async Task<UploadSessionResponse> GetUploadSession(string fileName, CancellationToken stoppingToken)
    {
        Dictionary<string, string> headers = new()
        {
            { "Authorization", $"Bearer {_session.AuthToken}" }
        };
        return await _httpClient.PostAsync<UploadSessionResponse>(_session.ApiUrl + "upload/session", new CreateUploadSessionRequest()
        {
            ConflictBehavior = ConflictBehaviors.Replace,
            Name = Path.GetFileName(fileName),
        }, headers: headers, ct: stoppingToken) ?? throw new Exception("Unable to create session");
    }
}