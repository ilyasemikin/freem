namespace Freem.Locking.Abstractions;

public interface IDistributedLocker
{
    Task<IDistributedLock> LockAsync(
        string key, 
        TimeSpan? lockTime = default,
        CancellationToken cancellationToken = default);
}
