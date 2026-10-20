using Freem.Api.Public.Configuration.Instances;

namespace Freem.Api.Public.Configuration;

public sealed class CompositeConfiguration
{
    public required ApiConfiguration Api { get; init; }
    
    public required OpenApiConfiguration OpenApi { get; init; }
    
    public required PostgresConfiguration Postgres { get; init; }
    public required RedisConfiguration Redis { get; init; }
    public required EgoHubIntegrationConfiguration EgoHubIntegration { get; init; }
}