using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using CategoryStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.CategoryStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class CategoryEntity : IRowVersionableEntity, IAuditableEntity
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public string? Name { get; set; }

    public required CategoryStatusDb Status { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }

    [Timestamp]
    public uint RowVersion { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
