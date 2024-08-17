namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

internal interface IAuditableEntity
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}
