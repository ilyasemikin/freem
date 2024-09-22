using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Collections.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class RunningRecordEntity : 
    IAuditableEntity, 
    IRowVersionableEntity,
    IEquatable<RunningRecordEntity>,
    ICloneable<RunningRecordEntity>
{
    public required string UserId { get; init; }
    
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTimeOffset StartAt { get; set; }

    public UserEntity? User { get; set; }
    public ICollection<ActivityEntity>? Activities { get; set; }
    public ICollection<TagEntity>? Tags { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public RunningRecordEntity Clone()
    {
        return new RunningRecordEntity
        {
            UserId = UserId,
            Name = Name,
            Description = Description,
            StartAt = StartAt,
            User = User,
            Activities = Activities?.ToList(),
            Tags = Tags?.ToList(),
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };
    }

    public bool Equals(RunningRecordEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            UserId == other.UserId && 
            Name == other.Name && 
            Description == other.Description && 
            StartAt.Equals(other.StartAt) && 
            Activities.NullableUnorderedEquals(other.Activities) && 
            Tags.NullableUnorderedEquals(other.Tags) && 
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is RunningRecordEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return UserId.GetHashCode();
    }
}
