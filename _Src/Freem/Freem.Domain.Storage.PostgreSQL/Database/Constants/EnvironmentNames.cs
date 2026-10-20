namespace Freem.Domain.Storage.PostgreSQL.Database.Constants;

internal static class EnvironmentNames
{
    public const string Schema = "core_entities";

    public static class Migrations
    {
        public const string Assembly = "Freem.Domain.Storage.PostgreSQL.Migrations";
        public const string HistoryTable = "__ef_migrations";
    }
}