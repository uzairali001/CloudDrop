namespace CloudDrop.App.Core.Services.General;
public static class RetryService
{

    public static async Task ExecuteAsync(Func<Task> operation, Action<RetryOptions>? options = default)
    {
        RetryOptions opts = new();
        options?.Invoke(opts);

        int currentAttempt = 0;
        TimeSpan delay = opts.InitialDelay;

        while (currentAttempt < opts.MaxAttempts)
        {
            try
            {
                await operation();
                return;
            }
            catch (Exception ex)
            {
                // Log the exception
            }

            currentAttempt++;
            if (currentAttempt < opts.MaxAttempts)
            {
                await Task.Delay(delay);
                delay = GetDelay(opts, delay);
            }
        }

        throw new Exception($"Failed to execute operation after {opts.MaxAttempts} attempts.");
    }

    public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, Action<RetryOptions>? options = default)
    {
        RetryOptions opts = new();
        options?.Invoke(opts);

        int currentAttempt = 0;
        TimeSpan delay = opts.InitialDelay;

        while (currentAttempt < opts.MaxAttempts)
        {
            try
            {
                T result = await operation();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
            }

            currentAttempt++;
            if (currentAttempt < opts.MaxAttempts)
            {
                await Task.Delay(delay);
                delay = GetDelay(opts, delay);
            }
        }

        throw new Exception($"Failed to execute operation after {opts.MaxAttempts} attempts.");
    }


    private static TimeSpan GetDelay(RetryOptions opts, TimeSpan delay)
    {
        return TimeSpan.FromTicks(Math.Min(opts.MaxDelay.Ticks, opts.RetryPolicy switch
        {
            RetryPolicies.ExponentialBackoff => delay.Ticks * 2,
            RetryPolicies.FixedInterval => delay.Ticks,
            _ => throw new NotImplementedException(),
        }));
    }
}


public class RetryOptions
{
    public int MaxAttempts { get; set; } = 3;
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromMinutes(1);

    public RetryPolicies RetryPolicy { get; set; } = RetryPolicies.ExponentialBackoff;
}

public enum RetryPolicies
{
    FixedInterval,
    ExponentialBackoff,
}