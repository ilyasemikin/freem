using Freem.Api.Public.Configuration;

namespace Freem.Api.Public.IntegrationTests.Infrastructure.Configuration;

public sealed class TestsConfiguration
{
    public required CompositeConfiguration WebApiApplication { get; init; }
}