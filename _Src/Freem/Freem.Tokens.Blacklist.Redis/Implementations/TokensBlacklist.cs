using Freem.Tokens.Abstractions;
using StackExchange.Redis;

namespace Freem.Tokens.Blacklist.Redis.Implementations;

public sealed class TokensBlacklist : ITokensBlacklist
{
    private readonly string _key;
    private readonly IConnectionMultiplexer _connection;

    public TokensBlacklist(string key, IConnectionMultiplexer connection)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(connection);
        
        _key = key;
        _connection = connection;
    }

    public async Task AddAsync(string token, CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();

        await db.SetAddAsync(_key, token);
    }

    public async Task<bool> ContainsAsync(string token, CancellationToken cancellationToken)
    {
        var db = _connection.GetDatabase();

        var value = await db.StringGetSetAsync(_key, token);
        return value.HasValue;
    }
}