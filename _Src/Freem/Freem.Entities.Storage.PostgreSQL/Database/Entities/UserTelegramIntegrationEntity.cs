using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserTelegramIntegrationEntity : 
    IAuditableEntity,
    IEquatable<UserTelegramIntegrationEntity>,
    ICloneable<UserTelegramIntegrationEntity>
{
    public required string UserId { get; init; }
    
    public required string TelegramUserId { get; init; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; set; }
    
    public UserEntity? User { get; set; }
    
    public bool Equals(UserTelegramIntegrationEntity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            UserId == other.UserId &&
            TelegramUserId == other.TelegramUserId &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
    }

    public UserTelegramIntegrationEntity Clone()
    {
        return new UserTelegramIntegrationEntity
        {
            UserId = UserId,
            TelegramUserId = TelegramUserId
        };
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is UserPasswordCredentialsEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return UserId.GetHashCode();
    }
}