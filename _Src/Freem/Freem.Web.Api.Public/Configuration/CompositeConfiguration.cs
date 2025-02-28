using Freem.Web.Api.Public.Configuration.Instances;

namespace Freem.Web.Api.Public.Configuration;

public sealed class CompositeConfiguration
{
    public required PostgresConfiguration Postgres { get; init; }
    public required RedisConfiguration Redis { get; init; }
    public required KafkaConfiguration Kafka { get; init; }
    
    public required PasswordCredentialsConfiguration PasswordCredentials { get; init; }
    
    public required CompositeTokensConfiguration Tokens { get; init; }
}