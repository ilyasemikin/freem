namespace Freem.Storage.Transactions.Abstractions;

public interface IStorageTransactionFactory
{
    Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default);
}
