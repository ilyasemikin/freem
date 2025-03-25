using Timeout = Freem.Timeouts.Models.Timeout;

namespace Freem.Locking.Abstractions;

public interface IDistributedLock : IAsyncDisposable
{
    DateTimeOffset? Expires { get; }
    
    Task ReleaseAsync(Timeout? timeout = default, CancellationToken cancellationToken = default);
}