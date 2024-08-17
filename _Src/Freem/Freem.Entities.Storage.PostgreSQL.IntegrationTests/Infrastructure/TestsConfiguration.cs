namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal sealed class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public string ConnectionString { get; }

    public TestsConfiguration(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        
        ConnectionString = connectionString;
    }
}
