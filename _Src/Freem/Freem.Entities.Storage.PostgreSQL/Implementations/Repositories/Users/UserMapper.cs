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
            Nickname = user.Nickname
        };
    }

    public static User MapToDomainEntity(this UserEntity entity)
    {
        var id = new UserIdentifier(entity.Id);
        return new User(id, entity.Nickname);
    }
}
