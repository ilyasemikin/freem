namespace Freem.Storage.Abstractions;

public interface IStorageTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task AbortAsync(CancellationToken cancellationToken = default);
    
    Task<bool> TryCommitAsync(CancellationToken cancellationToken = default);
    Task<bool> TryAbortAsync(CancellationToken cancellationToken = default);
}
