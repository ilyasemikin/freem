using Timeout = Freem.Locking.Timeouts.Models.Timeout;

namespace Freem.Locking;

public interface IDistributedLock : IAsyncDisposable
{
    DateTimeOffset? Expires { get; }
    
    Task ReleaseAsync(Timeout? timeout = default, CancellationToken cancellationToken = default);
}