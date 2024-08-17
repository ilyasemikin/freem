﻿using Freem.Entities.Storage.PostgreSQL.Database.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;

internal static class EntitiesNames
{
    public static class Users
    {
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

        public static class Constaints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string NameUserIdIndex = $"{Table}_{Properties.Name}_{NamingConvention.IndexSuffix}";
            public const string NameUserIdUnique = $"{Table}_{Properties.Name}_{NamingConvention.UniqueSuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.UniqueSuffix}";
        }
    }

    public static class Categories
    {
        public const string Table = "categories";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Status = "status";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constaints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
        }

        public static class Models
        {
            public const string CategoryStatus = "category_status";
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
            public const string StartAt = "start_at";
            public const string EndAt = "end_at";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constaints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TimePeriodCheck = $"{Table}_{Users.Table}_{NamingConvention.CheckSuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
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

        public static class Constaints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Table}_{NamingConvention.ForeignKeySuffix}";
        }

    }
}
