using Timeout = Freem.Timeouts.Models.Timeout;

namespace Freem.Timeouts;

public static class ExecutorWithTimeout
{
    public static async Task<bool> ExecuteAsync(
        Func<Task<bool>> function, Timeout? timeout, 
        CancellationToken cancellationToken = default)
    {
        timeout ??= new Timeout();

        var iteration = 0;
        while (true)
        {
            var result = await function();

            if (result || iteration == timeout.Repeats - 1)
                return result;
            
            iteration++;
            await Task.Delay(timeout.Period, cancellationToken);
        }
    }
}