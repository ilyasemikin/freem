﻿using Freem.Identifiers.Abstractions.Generators;
using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Exceptions;
using StackExchange.Redis;

namespace Freem.Locking.Redis.Implementations.Simple;

internal sealed class SimpleDistributedLocker : IDistributedLocker
{
    private readonly IConnectionMultiplexer _connection;
    private readonly IIdentifierGenerator<SimpleLockIdentifier> _idGenerator;

    public SimpleDistributedLocker(
        IConnectionMultiplexer connection, 
        IIdentifierGenerator<SimpleLockIdentifier> idGenerator)
    {
        _connection = connection;
        _idGenerator = idGenerator;
    }

    public async Task<SimpleDistributedLock> LockAsync(
        string key,
        TimeSpan? lockTime = default,
        CancellationToken cancellationToken = default)
    {
        var id = _idGenerator.Generate();

        var db = _connection.GetDatabase();
        var locked = await db.StringSetAsync(key, id, lockTime, When.NotExists);
        
        if (!locked)
            throw new CantLockException(key);
        
        return new SimpleDistributedLock(_connection, key, id);
    }
    
    async Task<IDistributedLock> IDistributedLocker.LockAsync(
        string key, 
        TimeSpan? lockTime, 
        CancellationToken cancellationToken)
    {
        return await LockAsync(key, lockTime, cancellationToken);
    }
}