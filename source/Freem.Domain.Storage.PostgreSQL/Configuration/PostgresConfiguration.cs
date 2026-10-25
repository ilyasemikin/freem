namespace Freem.Domain.Storage.PostgreSQL.Configuration;

public sealed class PostgresConfiguration
{
    public string ConnectionString { get; }

    public bool EnableSensitiveDataLogging { get; init; }

    public PostgresConfiguration(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        ConnectionString = connectionString;
    }
}
