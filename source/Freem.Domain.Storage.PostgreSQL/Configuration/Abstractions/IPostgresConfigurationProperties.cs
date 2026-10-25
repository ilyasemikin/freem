namespace Freem.Domain.Storage.PostgreSQL.Configuration.Abstractions;

public interface IPostgresConfigurationProperties
{
    PostgresConfiguration Postgres { get; }
}
