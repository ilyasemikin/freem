namespace Freem.Locking.Abstractions.Extensions;

public static class DistributedLockerExtensions
{
    public static async Task<IDistributedLock> LockAsync(
        this IDistributedLocker locker,
        string key,
        CancellationToken cancellationToken)
    {
        return await locker.LockAsync(key, cancellationToken: cancellationToken);
    }
}