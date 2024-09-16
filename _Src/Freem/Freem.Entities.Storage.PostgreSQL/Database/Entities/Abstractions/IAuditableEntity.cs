namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

public interface IAuditableEntity
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}