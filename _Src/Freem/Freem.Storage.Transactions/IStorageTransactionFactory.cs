namespace Freem.Storage.Transactions;

public interface IStorageTransactionFactory
{
    Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default);
}
