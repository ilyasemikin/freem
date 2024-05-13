using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Freem.Entities.Storage.PostgreSQL.Migrations;

internal class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length == 0 
            ? string.Empty 
            : args[0];

        var builder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(connectionString, b => 
            {
                b.MigrationsAssembly(EnvironmentNames.Migrations.Assembly);
                b.MigrationsHistoryTable(EnvironmentNames.Migrations.HistoryTable, EnvironmentNames.Schema);
            })
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

        return new DatabaseContext(builder.Options);
    }
}