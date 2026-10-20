using Freem.Locking;
using Freem.Locking.Exceptions;
using Freem.Locking.Timeouts;
using StackExchange.Redis;
using Timeout = Freem.Locking.Timeouts.Models.Timeout;

namespace Freem.Locking.Redis.Implementations.Simple;

internal class SimpleDistributedLock : IDistributedLock
{
    private static readonly LuaScript Script;
    
    private readonly IConnectionMultiplexer _connection;

    private bool _released;
    
    public string Key { get; }
    public string Token { get; }

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
    
    public SimpleDistributedLock(IConnectionMultiplexer connection, string key, string token)
    {
        _released = false;
        _connection = connection;
        
        Key = key;
        Token = token;
    }
    
    public async Task ReleaseAsync(Timeout? timeout = default, CancellationToken cancellationToken = default)
    {
        if (_released)
            throw new CantReleaseException(Key);
        
        var db = _connection.GetDatabase();

        var parameters = new { key = Key, id = Token };
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