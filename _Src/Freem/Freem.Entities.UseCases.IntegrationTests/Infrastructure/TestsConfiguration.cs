using Freem.Configurations;

namespace Freem.Entities.UseCases.IntegrationTests.Infrastructure;

internal sealed class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public string PostgresConnectionString { get; }
    
    public TestsConfiguration(string postgresConnectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(postgresConnectionString);
        
        PostgresConnectionString = postgresConnectionString;
    }

    public static TestsConfiguration Read()
    {
        return Configuration.ReadFromJsonFile<TestsConfiguration>(DefaultFileName);
    }
}