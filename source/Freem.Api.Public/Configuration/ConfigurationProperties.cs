using Freem.Api.Public.Configuration.Subsystems;
using Freem.Domain.Storage.PostgreSQL.Configuration;
using Freem.Domain.Storage.PostgreSQL.Configuration.Abstractions;
using Utilities.App;
using Utilities.App.Configuration.Abstractions;
using Utilities.Logging.OpenSearch.Configuration;
using Utilities.Logging.OpenSearch.Configuration.Abstractions;

namespace Freem.Api.Public.Configuration;

internal sealed class ConfigurationProperties :
    IAppConfigurationProperties,
    IPostgresConfigurationProperties,
    IOpenSearchConfigurationProperties
{
    public required AppInfo App { get; init; }
    public required CorsConfigurationProperties Cors { get; init; }
    public required PostgresConfiguration Postgres { get; init; }
    public required RedisConfigurationProperties Redis { get; init; }
    public required OpenSearchConfiguration OpenSearch { get; init; }

    public required IntegrationsConfigurationProperties Integrations { get; init; }

    public OpenApiConfigurationProperties? OpenApi { get; init; }
}
