using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using ActivityStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class ActivityEntity : IAuditableEntity, IRowVersionableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public string? Name { get; set; }

    public required ActivityStatusDb Status { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }
}
