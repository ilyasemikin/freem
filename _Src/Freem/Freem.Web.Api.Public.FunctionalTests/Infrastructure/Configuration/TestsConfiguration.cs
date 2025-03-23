using Freem.Web.Api.Public.Configuration;

namespace Freem.Web.Api.Public.FunctionalTests.Infrastructure.Configuration;

public sealed class TestsConfiguration
{
    public required CompositeConfiguration WebApiApplication { get; init; }
}