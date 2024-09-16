namespace Freem.Storage.Abstractions;

public interface IStorageTransactionFactory
{
    Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default);
}
