namespace Freem.Tokens.Blacklist.Redis.DependencyInjection;

public sealed class RedisConfiguration
{
    public string ConnectionString { get; }
    public string BlacklistKey { get; }
    
    public RedisConfiguration(string connectionString, string blacklistKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(blacklistKey);
        
        ConnectionString = connectionString;
        BlacklistKey = blacklistKey;
    }
}