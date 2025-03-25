using Freem.Configurations;

namespace Freem.Locking.Redis.IntegrationTests.Infrastructure;

public class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";
    
    public string RedisConnectionString { get; }

    public TestsConfiguration(string redisConnectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(redisConnectionString);
        
        RedisConnectionString = redisConnectionString;
    }

    public static TestsConfiguration Read()
    {
        return Configuration.ReadFromJsonFile<TestsConfiguration>(DefaultFileName);
    }
}