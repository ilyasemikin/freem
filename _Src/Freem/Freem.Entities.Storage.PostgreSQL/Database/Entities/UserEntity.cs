using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserEntity : IRowVersionableEntity, ISoftDeletedEntity
{
    public required string Id { get; init; }

    public required string Nickname { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }

    public DateTimeOffset? DeletedAt { get; set; }
}
