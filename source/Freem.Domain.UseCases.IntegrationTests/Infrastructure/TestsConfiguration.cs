using Microsoft.Extensions.Configuration;

namespace Freem.Domain.UseCases.IntegrationTests.Infrastructure;

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
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(DefaultFileName)
            .Build();

        var result = configuration.Get<TestsConfiguration>();
        if (result is null)
            throw new InvalidOperationException($"Can't read tests configuration from \"{DefaultFileName}\"");

        return result;
    }
}