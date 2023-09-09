namespace CloudDrop.ConsoleApp;
public abstract class PeriodicBackgroundService : BackgroundService
{
    protected TimeSpan _period;
    protected readonly ILogger<Worker> _logger;

    public PeriodicBackgroundService(ILogger<Worker> logger, TimeSpan? period = default)
    {
        _logger = logger;
        _period = period ?? TimeSpan.FromMinutes(1);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_period);
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await OnTickAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"BackgroundService Error with exception message {ex.Message}.");
            }
        }
    }

    protected abstract Task OnTickAsync(CancellationToken stoppingToken);
}
