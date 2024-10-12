namespace Freem.Storage.Abstractions.Helpers.Extensions;

public static class StorageTransactionRunnerExtensions
{
    public static async Task RunAsync(
        this StorageTransactionRunner runner, 
        Func<Task> function, 
        CancellationToken cancellationToken = default)
    {
        await runner.RunAsync(async _ => await function(), cancellationToken);
    }

    public static async Task<T> RunAsync<T>(
        this StorageTransactionRunner runner,
        Func<Task<T>> function,
        CancellationToken cancellationToken = default)
    {
        return await runner.RunAsync(async _ => await function(), cancellationToken);
    }
}