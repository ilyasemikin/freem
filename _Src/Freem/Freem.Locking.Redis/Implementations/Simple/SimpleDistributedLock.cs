using Freem.Locking.Abstractions;
using StackExchange.Redis;

namespace Freem.Locking.Redis.Implementations.Simple;

internal class SimpleDistributedLock : IDistributedLock
{
    private readonly IConnectionMultiplexer _connection;

    private bool _released;
    
    public string Key { get; }
    public SimpleLockIdentifier Identifier { get; }

    public DateTimeOffset? Expires { get; init; }

    public SimpleDistributedLock(IConnectionMultiplexer connection, string key, SimpleLockIdentifier identifier)
    {
        _released = false;
        _connection = connection;
        
        Key = key;
        Identifier = identifier;
    }
    
    public async Task ReleaseAsync(CancellationToken cancellationToken = default)
    {
        if (_released)
            return;
        
        var db = _connection.GetDatabase();

        var prepared = LuaScript.Prepare(
            "if redis.call(\"get\", @key) == @id then\n" +
            "\treturn redis.call(\"del\", @key)\n" +
            "else\n" +
            "\treturn 0\n" +
            "end");

        var result = await db.ScriptEvaluateAsync(prepared, new { key = Key, id = Identifier.ToString() });
        if (!result.IsNull)
            _released = true;
    }
    
    public async ValueTask DisposeAsync()
    {
        await ReleaseAsync();
    }
}