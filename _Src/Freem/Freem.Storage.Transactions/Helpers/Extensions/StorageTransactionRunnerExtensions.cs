using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Storage.Transactions.Helpers.Extensions;

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

    public static async Task<Result<Error>> RunAsync(
        this StorageTransactionRunner runner,
        Func<Task<Result<Error>>> function,
        CancellationToken cancellationToken = default)
    {
        return await runner.RunAsync(async _ => await function(), cancellationToken);
    }

    public static async Task<Result<T, Error>> RunAsync<T>(
        this StorageTransactionRunner runner,
        Func<Task<Result<T, Error>>> function,
        CancellationToken cancellationToken = default)
    {
        return await runner.RunAsync(async _ => await function(), cancellationToken);
    }
}
