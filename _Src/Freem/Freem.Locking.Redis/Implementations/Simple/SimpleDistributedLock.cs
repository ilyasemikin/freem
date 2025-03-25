using Freem.Locking.Abstractions;
using Freem.Locking.Abstractions.Exceptions;
using Freem.Timeouts;
using StackExchange.Redis;
using Timeout = Freem.Timeouts.Models.Timeout;

namespace Freem.Locking.Redis.Implementations.Simple;

internal class SimpleDistributedLock : IDistributedLock
{
    private static readonly LuaScript Script;
    
    private readonly IConnectionMultiplexer _connection;

    private bool _released;
    
    public string Key { get; }
    public SimpleLockIdentifier Identifier { get; }

    public DateTimeOffset? Expires { get; init; }

    static SimpleDistributedLock()
    {
        Script = LuaScript.Prepare(
            "local result = redis.call(\"get\", @key)\n" +
            "if result == @id then\n" +
            "\treturn redis.call(\"del\", @key)\n" +
            "else\n" +
            "\treturn 0\n" +
            "end");
    }
    
    public SimpleDistributedLock(IConnectionMultiplexer connection, string key, SimpleLockIdentifier identifier)
    {
        _released = false;
        _connection = connection;
        
        Key = key;
        Identifier = identifier;
    }
    
    public async Task ReleaseAsync(Timeout? timeout = default, CancellationToken cancellationToken = default)
    {
        if (_released)
            throw new CantReleaseException(Key);
        
        var db = _connection.GetDatabase();

        var parameters = new { key = Key, id = Identifier.ToString() };
        _released = await ExecutorWithTimeout.ExecuteAsync(TryRelease, timeout, cancellationToken);
        
        if (!_released)
            throw new CantReleaseException(Key);
        
        return;

        async Task<bool> TryRelease()
        {
            var result = await db.ScriptEvaluateAsync(Script, parameters);
            return !result.IsNull;
        }
    }
    
    public async ValueTask DisposeAsync()
    {
        if (!_released)
            await ReleaseAsync();
    }
}