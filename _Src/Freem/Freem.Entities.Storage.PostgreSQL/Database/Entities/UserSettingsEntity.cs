using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserSettingsEntity :
    IAuditableEntity,
    IEquatable<UserSettingsEntity>,
    ICloneable<UserSettingsEntity>
{
    public required string UserId { get; init; }

    public required long UtcOffsetTicks { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    [Timestamp] 
    public uint RowVersion { get; private set; }

    public UserEntity? User { get; set; }

    public UserSettingsEntity Clone()
    {
        return new UserSettingsEntity
        {
            UserId = UserId,
            UtcOffsetTicks = UtcOffsetTicks,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };
    }
    
    public bool Equals(UserSettingsEntity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            UserId == other.UserId &&
            UtcOffsetTicks == other.UtcOffsetTicks &&
            CreatedAt == other.CreatedAt &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }
    
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is TagEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return UserId.GetHashCode();
    }
}