namespace Freem.Api.Public.Configuration.Subsystems;

internal sealed class RedisConfigurationProperties
{
    public string ConnectionString { get; }

    public RedisConfigurationProperties(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        
        ConnectionString = connectionString;
    }
}
