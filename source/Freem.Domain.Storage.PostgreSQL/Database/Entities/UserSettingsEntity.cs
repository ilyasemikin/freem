using System.ComponentModel.DataAnnotations;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities;

internal sealed class UserSettingsEntity :
    IAuditableEntity,
    IEquatable<UserSettingsEntity>
{
    public required string UserId { get; init; }

    public required long TimeZoneOffsetTicks { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    [Timestamp] 
    public uint RowVersion { get; private set; }

    public UserEntity? User { get; set; }
    
    public bool Equals(UserSettingsEntity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            UserId == other.UserId &&
            TimeZoneOffsetTicks == other.TimeZoneOffsetTicks &&
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