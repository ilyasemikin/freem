namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

internal interface ISoftDeletedEntity
{
    DateTimeOffset? DeletedAt { get; set; }
}
