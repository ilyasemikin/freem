using Freem.Locking;
using Freem.Locking.Exceptions;
using Freem.Locking.Timeouts;
using StackExchange.Redis;
using Timeout = Freem.Locking.Timeouts.Models.Timeout;

namespace Freem.Locking.Redis.Implementations.Simple;

internal sealed class SimpleDistributedLocker : IDistributedLocker
{
    private readonly IConnectionMultiplexer _connection;

    public SimpleDistributedLocker(IConnectionMultiplexer connection)
    {
        _connection = connection;
    }

    public async Task<SimpleDistributedLock> LockAsync(
        string key,
        TimeSpan? lockTime = default,
        Timeout? timeout = default,
        CancellationToken cancellationToken = default)
    {
        var token = Guid.NewGuid().ToString("N");

        var db = _connection.GetDatabase();
        var locked = await ExecutorWithTimeout.ExecuteAsync(
            async () => await db.StringSetAsync(key, token, lockTime, When.NotExists),
            timeout, cancellationToken);

        if (!locked)
            throw new CantLockException(key);

        return new SimpleDistributedLock(_connection, key, token);
    }
    
    async Task<IDistributedLock> IDistributedLocker.LockAsync(
        string key, 
        TimeSpan? lockTime, 
        Timeout? timeout,
        CancellationToken cancellationToken)
    {
        return await LockAsync(key, lockTime, timeout, cancellationToken);
    }
}