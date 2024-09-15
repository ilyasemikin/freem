using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using Freem.Clones;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserEntity : 
    IAuditableEntity, 
    ISoftDeletedEntity, 
    IRowVersionableEntity,
    IEquatable<UserEntity>,
    ICloneable<UserEntity>
{
    public required string Id { get; init; }

    public required string Nickname { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public UserEntity Clone()
    {
        return new UserEntity
        {
            Id = Id,
            Nickname = Nickname,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            DeletedAt = DeletedAt,
        };
    }

    public bool Equals(UserEntity? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;
        return 
            Id == other.Id && 
            Nickname == other.Nickname &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue &&
            DeletedAt.HasValue == other.DeletedAt.HasValue;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is UserEntity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
