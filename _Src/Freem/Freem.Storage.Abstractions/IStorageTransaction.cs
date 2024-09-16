namespace Freem.Storage.Abstractions;

public interface IStorageTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken);
    Task AbortAsync(CancellationToken cancellationToken);
}
