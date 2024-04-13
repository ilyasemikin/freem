namespace Freem.Storage.Abstractions;

public interface IStorageTransaction
{
    Task AbortAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
}
