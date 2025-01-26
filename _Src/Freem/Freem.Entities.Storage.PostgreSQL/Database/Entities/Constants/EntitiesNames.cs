using Freem.Entities.Storage.PostgreSQL.Database.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;

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

    public static class UsersLoginCredentials
    {
        public const string EntityName = "user_password_credentials";
        public const string Table = "user_password_credentials";

        public static class Properties
        {
            public const string UserId = "user_id";
            public const string Login = "login";
            public const string HashAlgorithm = "password_hash_algorithm";
            public const string PasswordHash = "password_hash";
            public const string PasswordSalt = "password_salt";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string LoginIndex = $"{Table}_{Properties.Login}_{NamingConvention.IndexSuffix}";
            public const string LoginUnique = $"{Table}_{Properties.Login}_{NamingConvention.UniqueSuffix}";
        }
    }

    public static class UserTelegramIntegration
    {
        public const string EntityName = "user_telegram_integration";
        public const string Table = "user_telegram_integrations";

        public static class Properties
        {
            public const string UserId = "user_id";
            public const string TelegramUserId = "telegram_user_id";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TelegramUserIdIndex = $"{Table}_{Properties.TelegramUserId}_{NamingConvention.IndexSuffix}";
            public const string TelegramUserIdUnique = $"{Table}_{Properties.TelegramUserId}_{NamingConvention.UniqueSuffix}";
        }
    }

    public static class UserSettings
    {
        public const string EntityName = "user_settings";
        public const string Table = "user_settings";

        public static class Properties
        {
            public const string UserId = "user_id";
            public const string UtcOffsetTicks = "utc_offset_ticks";
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
        public const string EntityName = "tag";
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
        public const string EntityName = "activity";
        public const string Table = "activities";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
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
        public const string EntityName = "record";
        public const string Table = "records";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Description = "description";
            public const string StartAt = "start_at";
            public const string EndAt = "end_at";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TimePeriodCheck = $"{Table}_{Users.Table}_{NamingConvention.CheckSuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RunningRecords
    {
        public const string EntityName = "running_record";
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
            public const string EntityName = "entity_name";
            public const string EntityId = "entity_id";
            public const string Action = "action";
            public const string AdditionalData = "additional_data";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string EntityNameCheck = $"{Table}_{Properties.EntityName}_{NamingConvention.CheckSuffix}";
        }
    }
}
