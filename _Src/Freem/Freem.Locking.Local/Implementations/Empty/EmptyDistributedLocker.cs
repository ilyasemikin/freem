using Freem.Locking.Abstractions;
using Timeout = Freem.Timeouts.Models.Timeout;

namespace Freem.Locking.Local.Implementations.Empty;

internal sealed class EmptyDistributedLocker : IDistributedLocker
{
    public async Task<IDistributedLock> LockAsync(
        string key, TimeSpan? lockTime, Timeout? timeout,
        CancellationToken cancellationToken = default)
    {
        var @lock = EmptyDistributedLock.Instance;
        return await Task.FromResult(@lock);
    }
}