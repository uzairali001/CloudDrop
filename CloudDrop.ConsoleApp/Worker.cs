using CloudDrop.App.Core.Services.General;

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CloudDrop.ConsoleApp;

public class Worker : PeriodicBackgroundService
{
    public Worker(ILogger<Worker> logger) : base(logger)
    {
        _period = TimeSpan.FromSeconds(2);
    }


    protected override async Task OnTickAsync(CancellationToken stoppingToken)
    {
        string filePath = "TestFile.txt";
        var chunks = FileChunkService.ChunkAsync(filePath, cancellationToken: stoppingToken);

        await foreach (var item in chunks)
        {
            _logger.LogInformation("Content: {content}", MemoryToString.ToString(item, Encoding.UTF8));
            await Task.Delay(1000, stoppingToken);
        }
    }

    public static class MemoryToString
    {
        public static string ToString(Memory<byte> memory, Encoding encoding)
        {
            var stringBuilder = new StringBuilder();

            foreach (byte b in memory.ToArray())
            {
                stringBuilder.Append(encoding.GetString(new byte[] { b }));
            }

            return stringBuilder.ToString();
        }
    }
}

