using Microsoft.Extensions.Configuration;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal sealed class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public string ConnectionString { get; }

    public TestsConfiguration(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        
        ConnectionString = connectionString;
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
