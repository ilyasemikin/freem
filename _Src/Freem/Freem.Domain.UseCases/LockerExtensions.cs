using Utilities.Errors;
using Utilities.Locking;
using Utilities.Primitives;

namespace Freem.Domain.UseCases;

internal static class LockerExtensions
{
    private static readonly TimeSpan Period = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    public static async Task<Result<ILock, Error>> LockAsync(
        this ILocker locker,
        LockKey key,
        CancellationToken cancellationToken = default)
    {
        return await locker.LockAsync(key, Period, Timeout, cancellationToken);
    }
}
