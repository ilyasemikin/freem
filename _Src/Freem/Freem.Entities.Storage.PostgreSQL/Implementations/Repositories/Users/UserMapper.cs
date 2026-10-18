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
            Settings = user.Settings.MapToDatabaseEntity(user.Id)
        };
    }

    public static UserSettingsEntity MapToDatabaseEntity(this UserSettings settings, UserIdentifier userId)
    {
        return new UserSettingsEntity
        {
            UserId = userId,
            UtcOffsetTicks = settings.DayUtcOffset.Ticks
        };
    }

    public static User MapToDomainEntity(this UserEntity entity)
    {
        var id = new UserIdentifier(entity.Id);
        var user = new User(id, entity.Nickname);

        if (entity.Settings is not null)
            user.Settings = entity.Settings.MapToDomainEntity();

        return user;
    }

    public static UserSettings MapToDomainEntity(this UserSettingsEntity entity)
    {
        var offset = new TimeSpan(entity.UtcOffsetTicks);
        return new UserSettings()
        {
            DayUtcOffset = offset
        };
    }
}
