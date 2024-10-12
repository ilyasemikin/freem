using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Exceptions;
using Freem.Locking.Redis.DependencyInjection.Microsoft;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.Implementations.Simple;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Freem.Locking.Redis.IntegrationTests.Tests.Implementations;

public sealed class SimpleDistributedLockerTests : IDisposable
{
    private const string Key = "key";
    
    private readonly IConnectionMultiplexer _connection;
    private readonly SimpleDistributedLocker _locker;
    
    private bool _disposed;

    public SimpleDistributedLockerTests()
    {
        var connection = new RedisConfiguration("192.168.1.2:6379,allowAdmin=true");

        var services = new ServiceCollection();

        services.AddSimpleRedisDistributedLocks(connection);

        var provider = services.BuildServiceProvider();
        
        _connection = provider.GetRequiredService<IConnectionMultiplexer>();
        _locker = provider.GetRequiredService<SimpleDistributedLocker>();
    }

    [Fact]
    public async Task Locker_ShouldLockSuccess()
    {
        await using var @lock = await _locker.LockAsync(Key, TimeSpan.FromMinutes(1));

        var db = _connection.GetDatabase();
        var redisValue = db.StringGet(Key);
        
        Assert.True(redisValue.HasValue);

        var value = (string?)redisValue;
        Assert.NotNull(value);
        Assert.Equal(@lock.Identifier, value);
    }

    [Fact]
    public async Task Locker_ShouldLockFailure_WhenOtherLockExists()
    {
        await using var @lock = await _locker.LockAsync(Key);

        var exception = await Record.ExceptionAsync(async () => await _locker.LockAsync(Key));
        
        Assert.NotNull(exception);
        Assert.IsType<CantLockException>(exception);
        Assert.Equal(Key, ((CantLockException)exception).Key);
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        var server = _connection.GetServer("192.168.1.2:6379");
        server.FlushDatabase();
        
        _connection.Dispose();

        _disposed = true;
    }
}