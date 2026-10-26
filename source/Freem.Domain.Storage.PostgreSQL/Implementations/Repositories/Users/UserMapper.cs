using Freem.Domain.Entities.Users;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using DatabaseWeekStart = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.WeekStart;
using EntityWeekStart = Freem.Domain.Users.Models.WeekStart;

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
            TimeZoneOffsetTicks = settings.TimeZoneOffset.Ticks,
            WeekStart = settings.WeekStart.MapToDatabaseEntityWeekStart()
        };
    }

    public static DatabaseWeekStart MapToDatabaseEntityWeekStart(this EntityWeekStart weekStart)
    {
        return weekStart.ToValue() switch
        {
            EntityWeekStart.Value.Monday => DatabaseWeekStart.Monday,
            EntityWeekStart.Value.Sunday => DatabaseWeekStart.Sunday,
            _ => throw new ArgumentOutOfRangeException()
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
        var value = new TimeSpan(entity.TimeZoneOffsetTicks);
        var offset = new TimeZoneOffset(value);
        var weekStart = entity.WeekStart.MapToDomainEntityWeekStart();
        return new UserSettings()
        {
            TimeZoneOffset = offset,
            WeekStart = weekStart
        };
    }

    public static EntityWeekStart MapToDomainEntityWeekStart(this DatabaseWeekStart weekStart)
    {
        return weekStart switch
        {
            DatabaseWeekStart.Monday => EntityWeekStart.Monday,
            DatabaseWeekStart.Sunday => EntityWeekStart.Sunday,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
