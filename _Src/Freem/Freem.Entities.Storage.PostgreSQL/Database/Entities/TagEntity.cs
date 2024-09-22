using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class TagEntity : 
    IAuditableEntity, 
    IRowVersionableEntity,
    IEquatable<TagEntity>,
    ICloneable<TagEntity>
{
    public required string Id { get; init; }
    public required string UserId { get; init; }

    public required string Name { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<ActivityEntity>? Activities { get; set; }
    public ICollection<RecordEntity>? Records { get; set; }
    public ICollection<RunningRecordEntity>? RunningRecords { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public TagEntity Clone()
    {
        return new TagEntity
        {
            Id = Id,
            UserId = UserId,
            Name = Name,
            User = User,
            Activities = Activities?.ToList(),
            Records = Records?.ToList(),
            RunningRecords = RunningRecords?.ToList(),
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }

    public bool Equals(TagEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            Id == other.Id && 
            UserId == other.UserId && 
            Name == other.Name &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TagEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
