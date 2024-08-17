using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserEntity : IRowVersionableEntity, IAuditableEntity, ISoftDeletedEntity
{
    public required string Id { get; init; }

    public required string Nickname { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
