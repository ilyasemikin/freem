using Timeout = Freem.Locking.Abstractions.Timeouts.Models.Timeout;

namespace Freem.Locking.Abstractions;

public interface IDistributedLocker
{
    Task<IDistributedLock> LockAsync(
        string key,
        TimeSpan? lockTime = default,
        Timeout? timeout = default,
        CancellationToken cancellationToken = default);
}
