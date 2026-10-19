using Microsoft.Extensions.Configuration;

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
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(DefaultFileName)
            .Build();

        var result = configuration.Get<TestsConfiguration>();
        if (result is null)
            throw new InvalidOperationException($"Can't read tests configuration from \"{DefaultFileName}\"");

        return result;
    }
}