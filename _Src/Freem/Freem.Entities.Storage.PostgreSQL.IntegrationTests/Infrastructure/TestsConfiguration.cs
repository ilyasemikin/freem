using Freem.Configurations;
using Freem.DependencyInjection.Microsoft;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal class TestsConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public required string ConnectionString { get; init; }
}
