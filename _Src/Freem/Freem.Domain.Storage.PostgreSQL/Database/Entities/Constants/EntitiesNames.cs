using Freem.Domain.Storage.PostgreSQL.Database.Constants;

namespace Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;

internal static class EntitiesNames
{
    public static class Users
    {
        public const string EntityName = "user";
        public const string Table = "users";

        public static class Properties
        {
            public const string Id = "id";
            public const string Nickname = "nickname";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
            public const string DeletedAt = "deleted_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
        }
    }

    public static class UserSettings
    {
        public const string Table = "user_settings";

        public static class Properties
        {
            public const string UserId = "user_id";
            public const string TimeZoneOffsetTicks = "time_zone_offset_ticks";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
        }
    }

    public static class Tags
    {
        public const string Table = "tags";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string NameIndex = $"{Table}_{Properties.Name}_{NamingConvention.IndexSuffix}";
            public const string NameUnique = $"{Table}_{Properties.Name}_{NamingConvention.UniqueSuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.UniqueSuffix}";
        }
    }

    public static class Activities
    {
        public const string Table = "activities";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Color = "color";
            public const string Status = "status";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
        }

        public static class Models
        {
            public const string Status = "activity_status";
        }
    }

    public static class Records
    {
        public const string Table = "records";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Description = "description";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RecordPeriods
    {
        public const string Table = "record_periods";

        public static class Properties
        {
            public const string Id = "id";
            public const string RecordId = "record_id";
            public const string Description = "description";
            public const string StartAt = "start_at";
            public const string EndAt = "end_at";
            public const string TimeZoneOffsetTicks = "time_zone_offset_ticks";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RecordsForeignKey = $"{Table}_{Records.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TimePeriodCheck = $"{Table}_{Properties.StartAt}_{Properties.EndAt}_{NamingConvention.CheckSuffix}";
            public const string RecordIdIndex = $"{Table}_{Properties.RecordId}_{NamingConvention.IndexSuffix}";
            public const string TimePeriodIndex = $"{Table}_{Properties.StartAt}_{Properties.EndAt}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RunningRecords
    {
        public const string Table = "running_records";

        public static class Properties
        {
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Description = "description";
            public const string StartAt = "start_at";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{NamingConvention.ForeignKeySuffix}";
        }
    }

    public static class Events
    {
        public const string Table = "events";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string EventType = "event_type";
            public const string EntityType = "entity_type";
            public const string EntityRef = "entity_ref";
            public const string OccurredAt = "occurred_at";
            public const string Payload = "payload";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string EntityTypeCheck = $"{Table}_{Properties.EntityType}_{NamingConvention.CheckSuffix}";
            public const string UserIdOccurredAtIndex = $"{Table}_{Properties.UserId}_{Properties.OccurredAt}_{NamingConvention.IndexSuffix}";
            public const string UserIdEntityIndex = $"{Table}_{Properties.UserId}_{Properties.EntityType}_{Properties.EntityRef}_{NamingConvention.IndexSuffix}";
        }
    }
}
