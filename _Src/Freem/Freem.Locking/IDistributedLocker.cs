using Timeout = Freem.Locking.Timeouts.Models.Timeout;

namespace Freem.Locking;

public interface IDistributedLocker
{
    Task<IDistributedLock> LockAsync(
        string key,
        TimeSpan? lockTime = default,
        Timeout? timeout = default,
        CancellationToken cancellationToken = default);
}
