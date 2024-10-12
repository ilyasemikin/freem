namespace Freem.Locking.Redis.DependencyInjection.Microsoft;

public sealed class RedisConfiguration
{
    public string Configuration { get; }
    
    public RedisConfiguration(string configuration)
    {
        Configuration = configuration;
    }
}