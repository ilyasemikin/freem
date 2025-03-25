using Freem.Entities.Storage.PostgreSQL.Database.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.Constants;

internal class RelationsNames
{
    public static class ActivitiesTags
    {
        public const string Table = $"{EntitiesNames.Activities.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string ActivityId = "activity_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string ActivitiesForeignKey = $"{Table}_{EntitiesNames.Activities.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
            public const string UserIdCheckTrigger = $"check_activities_tags_user_ids_trigger";
        }
    }

    public static class RecordsTags
    {
        public const string Table = $"{EntitiesNames.Records.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string RecordId = "record_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RecordsForeignKey = $"{Table}_{EntitiesNames.Records.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
            public const string UserIdCheckTrigger = $"check_records_tags_user_ids_trigger";
        }
    }

    public static class RunningRecordsTags
    {
        public const string Table = $"{EntitiesNames.RunningRecords.Table}_{EntitiesNames.Tags.Table}";

        public static class Properties
        {
            public const string RunningRecordId = $"user_id";
            public const string TagId = "tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RunningRecordsForeignKey = $"{Table}_{EntitiesNames.RunningRecords.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{EntitiesNames.Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagIdIndex = $"{Table}_{Properties.TagId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RecordsActivities
    {
        public const string Table = $"{EntitiesNames.Records.Table}_{EntitiesNames.Activities.Table}";

        public static class Properties
        {
            public const string RecordId = "record_id";
            public const string ActivityId = "activity_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RecordsForeignKey = $"{Table}_{EntitiesNames.Records.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ActivitiesForeignKey = $"{Table}_{EntitiesNames.Activities.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ActivityIdIndex = $"{Table}_{Properties.ActivityId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class RunningRecordsActivities
    {
        public const string Table = $"{EntitiesNames.RunningRecords.Table}_{EntitiesNames.Activities.Table}";

        public static class Properties
        {
            public const string RunningRecordId = "user_id";
            public const string ActivityId = "activity_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RunningRecordsForeignKey = $"{Table}_{EntitiesNames.RunningRecords.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ActivitiesForeignKey = $"{Table}_{EntitiesNames.Activities.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ActivityIdIndex = $"{Table}_{Properties.ActivityId}_{NamingConvention.IndexSuffix}";
        }
    }
}
