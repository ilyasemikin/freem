using Freem.Configurations;

namespace Freem.Entities.UseCases.IntegrationTests.Infrastructure;

internal sealed class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public string PostgresConnectionString { get; }
    public string RedisConnectionString { get; }
    
    public TestsConfiguration(string postgresConnectionString, string redisConnectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postgresConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(redisConnectionString);
        
        PostgresConnectionString = postgresConnectionString;
        RedisConnectionString = redisConnectionString;
    }

    public static TestsConfiguration Read()
    {
        return Configuration.ReadFromJsonFile<TestsConfiguration>(DefaultFileName);
    }
}