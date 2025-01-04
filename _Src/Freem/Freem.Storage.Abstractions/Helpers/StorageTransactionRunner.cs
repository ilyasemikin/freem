namespace Freem.Storage.Abstractions.Helpers;

public sealed class StorageTransactionRunner
{
    private readonly IStorageTransactionFactory _factory;

    public StorageTransactionRunner(IStorageTransactionFactory factory)
    {
        _factory = factory;
    }
    
    public async Task RunAsync(Func<IStorageTransaction, Task> function, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _factory.CreateAsync(cancellationToken);

        try
        {
            await function(transaction);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.TryAbortAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> RunAsync<T>(Func<IStorageTransaction, Task<T>> function, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _factory.CreateAsync(cancellationToken);

        try
        {
            var result = await function(transaction);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.AbortAsync(cancellationToken);
            throw;
        }
    }
}
