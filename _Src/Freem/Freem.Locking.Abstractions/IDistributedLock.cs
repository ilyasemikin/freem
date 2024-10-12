namespace Freem.Locking.Abstractions;

public interface IDistributedLock : IAsyncDisposable
{
    DateTimeOffset? Expires { get; }
    
    Task ReleaseAsync(CancellationToken cancellationToken = default);
}