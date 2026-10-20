using Freem.Domain.Entities.Users;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Users;

internal static class UserMapper
{
    public static UserEntity MapToDatabaseEntity(this User user)
    {
        return new UserEntity
        {
            Id = user.Id.ToString(),
            Nickname = user.Nickname.ToString(),
            Settings = user.Settings.MapToDatabaseEntity(user.Id)
        };
    }

    public static UserSettingsEntity MapToDatabaseEntity(this UserSettings settings, UserId userId)
    {
        return new UserSettingsEntity
        {
            UserId = userId.ToString(),
            UtcOffsetTicks = settings.DayUtcOffset.Ticks
        };
    }

    public static User MapToDomainEntity(this UserEntity entity)
    {
        var id = UserId.Parse(entity.Id, null);
        var nickname = Nickname.Parse(entity.Nickname, null);
        var user = new User(id, nickname);

        if (entity.Settings is not null)
            user.Settings = entity.Settings.MapToDomainEntity();

        return user;
    }

    public static UserSettings MapToDomainEntity(this UserSettingsEntity entity)
    {
        var value = new TimeSpan(entity.UtcOffsetTicks);
        var offset = new DayUtcOffset(value);
        return new UserSettings()
        {
            DayUtcOffset = offset
        };
    }
}
