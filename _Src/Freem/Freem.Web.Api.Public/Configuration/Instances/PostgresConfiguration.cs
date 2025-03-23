namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class PostgresConfiguration
{
    public required string ConnectionString { get; init; }
    public required bool EnableSensitiveDataLogging { get; init; }
}