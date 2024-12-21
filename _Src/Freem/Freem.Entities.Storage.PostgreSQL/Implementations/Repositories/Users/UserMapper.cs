using Freem.Crypto.Hashes.Abstractions.Models;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Users;

internal static class UserMapper
{
    public static UserEntity MapToDatabaseEntity(this User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Nickname = user.Nickname,
            PasswordCredentials = user.PasswordCredentials?.MapToDatabaseEntity(user.Id)
        };
    }

    public static UserPasswordCredentialsEntity MapToDatabaseEntity(
        this UserPasswordCredentials credentials, UserIdentifier userId)
    {
        return new UserPasswordCredentialsEntity
        {
            UserId = userId,
            Login = credentials.Login,
            HashAlgorithm = credentials.HashAlgorithm,
            PasswordHash = credentials.PasswordHash,
            PasswordSalt = credentials.SaltHash
        };
    }

    public static User MapToDomainEntity(this UserEntity entity)
    {
        var id = new UserIdentifier(entity.Id);
        return new User(id, entity.Nickname)
        {
            PasswordCredentials = entity.PasswordCredentials?.MapToDomainEntity()
        };
    }

    public static UserPasswordCredentials MapToDomainEntity(this UserPasswordCredentialsEntity entity)
    {
        var algorithm = new HashAlgorithm(entity.HashAlgorithm);
        return new UserPasswordCredentials(entity.Login, algorithm, entity.PasswordHash, entity.PasswordSalt);
    }
}
