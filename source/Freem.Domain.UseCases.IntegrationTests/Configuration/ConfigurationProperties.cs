using Freem.Domain.Storage.PostgreSQL.Configuration;
using Freem.Domain.Storage.PostgreSQL.Configuration.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Freem.Domain.UseCases.IntegrationTests.Configuration;

internal sealed class ConfigurationProperties : IPostgresConfigurationProperties
{
    public const string Filename = "configuration.json";

    public required PostgresConfiguration Postgres { get; init; }

    public static ConfigurationProperties Read()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(Filename)
            .Build()
            .Get<ConfigurationProperties>();

        return configuration ?? throw new InvalidOperationException("Configuration is null");
    }
}
