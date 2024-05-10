using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Freem.Entities.Storage.PostgreSQL.Migrations;

internal class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        if (args.Length == 0)
            throw new InvalidOperationException("Connection string must be provided as first argument");

        var connectionString = args[0];

        var builder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(connectionString, b => 
            {
                b.MigrationsAssembly("Freem.Entities.Storage.PostgreSQL.Migrations");
                b.MigrationsHistoryTable(Names.Tables.EFCoreMigrations, Names.Schema);
            })
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

        return new DatabaseContext(builder.Options);
    }
}