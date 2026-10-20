using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Storage.Transactions.Helpers;

public sealed class StorageTransactionRunner
{
    private readonly IStorageTransactionFactory _factory;

    private IStorageTransaction? _transaction;

    public StorageTransactionRunner(IStorageTransactionFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _factory = factory;
        _transaction = null;
    }

    public async Task RunAsync(Func<IStorageTransaction, Task> function, CancellationToken cancellationToken = default)
    {
        await RunAsync(async transaction =>
        {
            await function(transaction);
            return Result<bool, Error>.CreateSuccess(true);
        }, cancellationToken);
    }

    public async Task<T> RunAsync<T>(Func<IStorageTransaction, Task<T>> function, CancellationToken cancellationToken = default)
    {
        var result = await RunAsync(async transaction =>
        {
            var value = await function(transaction);
            return Result<T, Error>.CreateSuccess(value);
        }, cancellationToken);

        return result.Value!;
    }

    public async Task<Result<Error>> RunAsync(
        Func<IStorageTransaction, Task<Result<Error>>> function,
        CancellationToken cancellationToken = default)
    {
        var result = await RunAsync(async transaction =>
        {
            var inner = await function(transaction);
            return inner.Success
                ? Result<bool, Error>.CreateSuccess(true)
                : inner.Error;
        }, cancellationToken);

        return result.Success
            ? Result<Error>.CreateSuccess()
            : result.Error;
    }

    public async Task<Result<T, Error>> RunAsync<T>(
        Func<IStorageTransaction, Task<Result<T, Error>>> function,
        CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return await function(_transaction);

        await using var transaction = await _factory.CreateAsync(cancellationToken);
        _transaction = transaction;

        try
        {
            var result = await function(transaction);
            if (result.Success)
                await transaction.CommitAsync(cancellationToken);
            else
                await transaction.AbortAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.TryAbortAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction = null;
        }
    }
}
