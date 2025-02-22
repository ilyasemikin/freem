using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Collections.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using ActivityStatusDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class ActivityEntity : 
    IAuditableEntity, 
    IRowVersionableEntity, 
    IEquatable<ActivityEntity>,
    ICloneable<ActivityEntity>
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public required string Name { get; set; }

    public required ActivityStatusDb Status { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public ActivityEntity Clone()
    {
        return new ActivityEntity
        {
            Id = Id,
            UserId = UserId,
            Name = Name,
            Status = Status,
            User = User,
            Tags = Tags?.ToList(),
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }

    public bool Equals(ActivityEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return
            Id == other.Id &&
            UserId == other.UserId &&
            Name == other.Name &&
            Status == other.Status &&
            Tags.NullableUnorderedEquals(other.Tags) &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ActivityEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
