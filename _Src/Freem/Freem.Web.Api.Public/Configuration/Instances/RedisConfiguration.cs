namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class RedisConfiguration
{
    public required string ConnectionString { get; init; }
    public required string TokensBlackListKey { get; init; }
}