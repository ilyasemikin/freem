namespace Freem.Locking.Redis.DependencyInjection.Microsoft;

public sealed class RedisConfiguration
{
    public string ConnectionString { get; }
    
    public RedisConfiguration(string connectionString)
    {
        ConnectionString = connectionString;
    }
}