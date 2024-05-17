using Freem.Configurations;
using Freem.DependencyInjection.Microsoft;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;

internal static class DatabaseContextFactory
{
    public static DatabaseContext Create()
    {
        var configuration = Configuration.ReadFromJsonFile<TestsConfiguration>(TestsConfiguration.DefaultFileName);
        var connectionString = configuration.ConnectionString;

        return Services.Resolve<DatabaseContext>(services => services.AddPostgreSqlStorage(connectionString));
    }
}
