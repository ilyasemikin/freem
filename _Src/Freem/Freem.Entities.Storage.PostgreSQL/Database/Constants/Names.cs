namespace Freem.Entities.Storage.PostgreSQL.Database.Constants;

// Constraint naming convention from https://til.cybertec-postgresql.com/post/2019-09-02-Postgres-Constraint-Naming-Convention/
internal static class Names
{
    private const string PrimaryKeySuffix = "pk";
    private const string ForeignKeySuffix = "fk";
    private const string IndexSuffix = "idx";
    private const string UniqueSuffix = "unique";
    private const string CheckSuffix = "check";

    public const string Schema = "core_entities";

    public static class Tables
    {
        public const string Categories = "categories";
        public const string Records = "records";
        public const string RunningRecords = "running_records";
        public const string Tags = "tags";
        public const string Users = "users";

        public const string EFCoreMigrations = "__ef_core_migrations";
    }

    public static class Properties
    {
        public static class Categories
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Name = "name";
        }

        public static class Records
        {
            public const string Id = "id";
            public const string StartAt = "start_at";
            public const string EndAt = "end_at";
            public const string Name = "name";
            public const string Description = "description";
        }

        public static class RunningRecords
        {
            public const string UserId = "user_id";
            public const string Name = "name";
            public const string Description = "description";
            public const string StartAt = "start_at";
        }

        public static class Tags
        {
            public const string Id = "id";
            public const string Name = "name";
        }

        public static class Users
        {
            public const string Id = "id";
            public const string Nickname = "nickname";
        }
    }

    public static class Constrains
    {
        public static class Categories 
        {
            public const string PrimaryKey = $"{Tables.Categories}_{PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Tables.Categories}_{Tables.Users}_{ForeignKeySuffix}";
            public const string UserIdIndex = $"{Tables.Categories}_{Properties.Categories.UserId}_{IndexSuffix}";
        }

        public static class Records 
        {
            public const string PrimaryKey = $"{Tables.Records}_{PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Tables.Records}_{Tables.Users}_{ForeignKeySuffix}";

            public const string TimeRangeCheck = $"{Tables.Records}_time_range_{CheckSuffix}";
        }

        public static class RunningRecords
        {
            public const string PrimaryKey = $"{Tables.RunningRecords}_{PrimaryKeySuffix}";
            public const string UsersForeignKey = $"{Tables.RunningRecords}_{Tables.Users}_{ForeignKeySuffix}";
        }

        public static class Tags 
        {
            public const string PrimaryKey = $"{Tables.Tags}_{PrimaryKeySuffix}";
            public const string NameIndex = $"{Tables.Tags}_{Properties.Tags.Name}_{IndexSuffix}";
            public const string NameUnique = $"{Tables.Tags}_{Properties.Tags.Name}_{UniqueSuffix}";
        }

        public static class Users
        {
            public const string PrimaryKey = $"{Tables.Users}_{PrimaryKeySuffix}";
            public const string NicknameIndex = $"{Tables.Users}_{Properties.Users.Nickname}_{IndexSuffix}";
            public const string NicknameUnique = $"{Tables.Users}_{Properties.Users.Nickname}_{UniqueSuffix}";
        }
    }

    public static class Relations
    {
        public static class Tables
        {
            public const string CategoriesTags = $"{Names.Tables.Categories}_{Names.Tables.Tags}";
            public const string RecordsTags = $"{Names.Tables.Records}_{Names.Tables.Tags}";
            public const string RunningRecordsTags = $"{Names.Tables.RunningRecords}_{Names.Tables.Tags}";
            public const string RecordsCategories = $"{Names.Tables.Records}_{Names.Tables.Categories}";
            public const string RunningRecordCategories = $"{Names.Tables.RunningRecords}_{Names.Tables.Categories}";
        }

        public static class Properties
        {
            public static class CategoriesTags 
            {
                public const string CategoryId = "category_id";
                public const string TagId = "tag_id";
            }

            public static class RecordsTags
            {
                public const string RecordId = "record_id";
                public const string TagId = "tag_id";
            }

            public static class RunningRecordsTags
            {
                public const string RunningRecordUserId = "user_id";
                public const string TagId = "tag_id";
            }

            public static class RecordsCategories
            {
                public const string RecordId = "record_id";
                public const string CategoryId = "category_id";
            }

            public static class RunningRecordsCategories
            {
                public const string RunningRecordUserId = "user_id";
                public const string CategoryId = "category_id";
            }
        }

        public static class Constrains
        {
            public static class CategoriesTags
            {
                public const string PrimaryKey = $"{Tables.CategoriesTags}_{PrimaryKeySuffix}";
                public const string CategoriesForeignKey = $"{Tables.CategoriesTags}_{Names.Tables.Categories}_{ForeignKeySuffix}";
                public const string TagsForeignKey = $"{Tables.CategoriesTags}_{Names.Tables.Tags}_{ForeignKeySuffix}";
                public const string CategoryIdIndex = $"{Tables.CategoriesTags}_{Properties.CategoriesTags.CategoryId}_{IndexSuffix}";
                public const string TagIdIndex = $"{Tables.CategoriesTags}_{Properties.CategoriesTags.TagId}_{IndexSuffix}";
            }

            public static class RecordsTags
            {
                public const string PrimaryKey = $"{Tables.RecordsTags}_{PrimaryKeySuffix}";
                public const string RecordsForeignKey = $"{Tables.RecordsTags}_{Names.Tables.Records}_{ForeignKeySuffix}";
                public const string TagsForeignKey = $"{Tables.RecordsTags}_{Names.Tables.Tags}_{ForeignKeySuffix}";
                public const string RecordIdIndex = $"{Tables.RecordsTags}_{Properties.RecordsTags.RecordId}_{IndexSuffix}";
                public const string TagIdIndex = $"{Tables.RecordsTags}_{Properties.RecordsTags.TagId}_{IndexSuffix}";
            }

            public static class RunningRecordsTags
            {
                public const string PrimaryKey = $"{Tables.RunningRecordsTags}_{PrimaryKeySuffix}";
                public const string RunningRecordsForeignKey = $"{Tables.RunningRecordsTags}_{Names.Tables.RunningRecords}_{ForeignKeySuffix}";
                public const string TagsForeignKey = $"{Tables.RunningRecordsTags}_{Names.Tables.Tags}_{ForeignKeySuffix}";
                public const string RunningRecordUserIdIndex = $"{Tables.RunningRecordsTags}_{Properties.RunningRecordsTags.RunningRecordUserId}_{IndexSuffix}";
                public const string TagIdIndex = $"{Tables.RunningRecordsTags}_{Properties.RunningRecordsTags.TagId}_{IndexSuffix}";
            }

            public static class RecordsCategories
            {
                public const string PrimaryKey = $"{Tables.RecordsCategories}_{PrimaryKeySuffix}";
                public const string RecordsForeignKey = $"{Tables.RecordsCategories}_{Names.Tables.Records}_{ForeignKeySuffix}";
                public const string CategoriesForeignKey = $"{Tables.RecordsCategories}_{Names.Tables.Categories}_{ForeignKeySuffix}";
                public const string RecordIdIndex = $"{Tables.RecordsCategories}_{Properties.RecordsCategories.RecordId}_{IndexSuffix}";
                public const string CategoryIdIndex = $"{Tables.RecordsCategories}_{Properties.RecordsCategories.CategoryId}_{IndexSuffix}";
            }

            public static class RunningRecordsCategories
            {
                public const string PrimaryKey = $"{Tables.RunningRecordCategories}_{PrimaryKeySuffix}";
                public const string RunningRecordsForeignKey = $"{Tables.RunningRecordCategories}_{Names.Tables.RunningRecords}_{ForeignKeySuffix}";
                public const string CategoriesForeignKey = $"{Tables.RunningRecordCategories}_{Names.Tables.Categories}_{ForeignKeySuffix}";
                public const string RunningRecordUserIdIndex = $"{Tables.RunningRecordCategories}_{Properties.RunningRecordsCategories.RunningRecordUserId}_{IndexSuffix}";
                public const string CategoryIdIndex = $"{Tables.RunningRecordCategories}_{Properties.RunningRecordsCategories.CategoryId}_{IndexSuffix}";
            }
        }
    }
}