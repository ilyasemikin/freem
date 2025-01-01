using System.ComponentModel.DataAnnotations;
using Freem.Clones;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal sealed class UserPasswordCredentialsEntity :
    IAuditableEntity,
    IEquatable<UserPasswordCredentialsEntity>,
    ICloneable<UserPasswordCredentialsEntity>
{
    public required string UserId { get; init; }
    
    public required string Login { get; init; }
    public required string HashAlgorithm { get; init; }
    public required string PasswordHash { get; init; }
    public required string PasswordSalt { get; init; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    [Timestamp]
    public uint RowVersion { get; private set; }

    public UserEntity? User { get; set; }
    
    public UserPasswordCredentialsEntity Clone()
    {
        return new UserPasswordCredentialsEntity
        {
            UserId = UserId,
            Login = Login,
            HashAlgorithm = HashAlgorithm,
            PasswordHash = PasswordHash,
            PasswordSalt = PasswordSalt,
            User = User,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };
    }
    
    public bool Equals(UserPasswordCredentialsEntity? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return
            UserId == other.UserId &&
            Login == other.Login &&
            HashAlgorithm == other.HashAlgorithm &&
            PasswordHash == other.PasswordHash &&
            PasswordSalt == other.PasswordSalt &&
            UpdatedAt.HasValue == other.UpdatedAt.HasValue;
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