using Freem.Configurations;
using Freem.DependencyInjection.Microsoft;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal class InfrastructureConfiguration
{
    public const string DefaultFileName = "configuration.json";

    public required string ConnectionString { get; init; }

    public static DatabaseContext CreateDatabaseContext()
    {
        var configuration = Configuration.ReadFromJsonFile<InfrastructureConfiguration>(DefaultFileName);
        var connectionString = configuration.ConnectionString;

        return Services.Resolve<DatabaseContext>(services => services.AddPostgreSqlStorage(connectionString));
    }
}
