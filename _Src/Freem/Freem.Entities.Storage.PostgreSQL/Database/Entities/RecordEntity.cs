using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Collections.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class RecordEntity : 
    IAuditableEntity, 
    IRowVersionableEntity,
    IEquatable<RecordEntity>, 
    ICloneable<RecordEntity>
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<ActivityEntity>? Activities { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public RecordEntity Clone()
    {
        return new RecordEntity
        {
            Id = Id,
            UserId = UserId,
            Name = Name,
            Description = Description,
            StartAt = StartAt,
            EndAt = EndAt,
            User = User,
            Activities = Activities?.ToList(),
            Tags = Tags?.ToList(),
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }

    public bool Equals(RecordEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            Id == other.Id && 
            UserId == other.UserId && 
            Name == other.Name && 
            Description == other.Description && 
            StartAt.Equals(other.StartAt) && 
            EndAt.Equals(other.EndAt) && 
            Activities.NullableUnorderedEquals(other.Activities) && 
            Tags.NullableUnorderedEquals(other.Tags) &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RecordEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
