using Freem.Api.Public.Client.Configuration;
using Freem.Api.Public.Client.Configuration.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Configuration;

public sealed class ConfigurationProperties : IFreemClientConfigurationProperties
{
    public const string Filename = "configuration.json";

    public required ClientConfiguration FreemClient { get; init; }
}
