using Freem.Api.Public.Configuration.Subsystems;
using Freem.Domain.Storage.PostgreSQL.Configuration;
using Freem.Domain.Storage.PostgreSQL.Configuration.Abstractions;

namespace Freem.Api.Public.Configuration;

internal sealed class ConfigurationProperties : IPostgresConfigurationProperties
{
    public required CorsConfigurationProperties Cors { get; init; }
    public required PostgresConfiguration Postgres { get; init; }
    public required RedisConfigurationProperties Redis { get; init; }

    public required IntegrationsConfigurationProperties Integrations { get; init; }

    public OpenApiConfigurationProperties? OpenApi { get; init; }
}
