using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Api.Core.Models.Responses;
using CloudDrop.App.Core.Services.General;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using UzairAli.NetHttpClient;

namespace CloudDrop.ConsoleApp;

public class Worker : PeriodicBackgroundService
{
    private readonly IHttpClientService _httpClient;

    public Worker(ILogger<Worker> logger,
        IHttpClientService httpClient) : base(logger)
    {
        _period = TimeSpan.FromSeconds(1);
        _httpClient = httpClient;
    }


    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        try
        {
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri("http://localhost:5124/v1/"),
            };

            string filePath = "TestFile.txt";
            //int chunkSize = 320 * 1024 * 4; // 1.25 MiB
            int chunkSize = 320; // 1.25 MiB
            var chunks = FileChunkService.ChunkAsync(filePath, chunkSize, cancellationToken: stoppingToken);


            long startByte = 0;
            long fileSize = new FileInfo(filePath).Length;

            UploadSessionResponse sessionResponse = await _httpClient.PostAsync<UploadSessionResponse>("upload/session", new CreateUploadSessionRequest()
            {
                ConflictBehavior = Api.Core.Enums.ConflictBehaviors.Replace,
                Name = Path.GetFileName(filePath),
            }, ct: stoppingToken) ?? throw new Exception("Unable to create session");

            await foreach (var item in chunks)
            {
                ContentRangeHeaderValue contentRange = new(startByte, startByte + item.Length -1, fileSize);

                _logger.LogInformation($"Uploading bytes: {contentRange}");

                using (MultipartFormDataContent form = new())
                {
                    var byteContent = new ByteArrayContent(item.ToArray());

                    form.Add(byteContent, "chunk", Path.GetFileName(filePath));
                    form.Headers.Add("Content-Range", contentRange.ToString());

                    await RetryService.ExecuteAsync(async () =>
                    {
                        var response = await httpClient.PutAsync($"upload/{sessionResponse.SessionId}", form, stoppingToken);
                        response.EnsureSuccessStatusCode();
                    });
                }

                startByte += item.Length;
            }
        }
        catch (Exception ex)
        {

        }

        await Task.Delay(1000 * 60 * 60);
    }


}

public static class MemoryByteExtensions
{
    public static string ToStringContent(this Memory<byte> memory, Encoding? encoding = default)
    {
        return (encoding ?? Encoding.UTF8).GetString(memory.ToArray());
    }
}