namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Abstractions;

internal interface ISoftDeletedEntity
{
    DateTimeOffset? DeletedAt { get; set; }
}
