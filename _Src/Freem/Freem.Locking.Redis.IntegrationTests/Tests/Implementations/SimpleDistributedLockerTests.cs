using Freem.Locking.Abstractions.Exceptions;
using Freem.Locking.Redis.DependencyInjection.Microsoft;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.Implementations.Simple;
using Freem.Locking.Redis.IntegrationTests.Infrastructure;
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
        var configuration = TestsConfiguration.Read();
        
        var connection = new RedisConfiguration(configuration.RedisConnectionString);

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

    [Fact]
    public async Task Locker_ShouldUnlockSuccess_WhenCallRelease()
    {
        var @lock = await _locker.LockAsync(Key);

        var db = _connection.GetDatabase();
        var redisValue = db.StringGet(Key);
        
        Assert.True(redisValue.HasValue);
        
        await @lock.ReleaseAsync();
        
        redisValue = db.StringGet(Key);
        Assert.False(redisValue.HasValue);
    }

    [Fact]
    public async Task Locker_ShouldLockDisappear_WhenLockTimeIsGone()
    {
        var lockTime = TimeSpan.FromSeconds(5);
        
        var @lock = await _locker.LockAsync(Key, lockTime);
        GC.KeepAlive(@lock);
        
        await Task.Delay(lockTime);
        
        var db = _connection.GetDatabase();
        var redisValue = db.StringGet(Key);
        
        Assert.False(redisValue.HasValue);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        var servers = _connection.GetServers();
        foreach (var server in servers)
            server.FlushDatabase();
        
        _connection.Dispose();

        _disposed = true;
    }
}