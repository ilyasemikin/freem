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
            public const string StatusCheck = $"{Table}_{Properties.Status}_{NamingConvention.CheckSuffix}";
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

    public static class ImportDocuments
    {
        public const string Table = "import_documents";

        public static class Properties
        {
            public const string Id = "id";
            public const string UserId = "user_id";
            public const string Format = "format";
            public const string FileName = "file_name";
            public const string TimeZoneOffsetTicks = "time_zone_offset_ticks";
            public const string Status = "status";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string StatusCheck = $"{Table}_{Properties.Status}_{NamingConvention.CheckSuffix}";
            public const string UsersForeignKey = $"{Table}_{Users.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string UserIdIndex = $"{Table}_{Properties.UserId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class ImportDocumentTags
    {
        public const string Table = "import_document_tags";

        public static class Properties
        {
            public const string DocumentId = "document_id";
            public const string SourceId = "source_id";
            public const string Name = "name";
            public const string CreateNew = "create_new";
            public const string ResolvedTagId = "resolved_tag_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string DocumentsForeignKey = $"{Table}_{ImportDocuments.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TagsForeignKey = $"{Table}_{Tags.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ResolutionCheck = $"{Table}_resolution_{NamingConvention.CheckSuffix}";
            public const string ResolvedTagIdIndex = $"{Table}_{Properties.ResolvedTagId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class ImportDocumentActivities
    {
        public const string Table = "import_document_activities";

        public static class Properties
        {
            public const string DocumentId = "document_id";
            public const string SourceId = "source_id";
            public const string Name = "name";
            public const string Color = "color";
            public const string Status = "status";
            public const string CreateNew = "create_new";
            public const string ResolvedActivityId = "resolved_activity_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string DocumentsForeignKey = $"{Table}_{ImportDocuments.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ActivitiesForeignKey = $"{Table}_{Activities.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string ResolutionCheck = $"{Table}_resolution_{NamingConvention.CheckSuffix}";
            public const string ResolvedActivityIdIndex = $"{Table}_{Properties.ResolvedActivityId}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class ImportDocumentActivityTags
    {
        public const string Table = "import_document_activity_tags";

        public static class Properties
        {
            public const string DocumentId = "document_id";
            public const string ActivitySourceId = "activity_source_id";
            public const string TagSourceId = "tag_source_id";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string ActivitiesForeignKey = $"{Table}_{ImportDocumentActivities.Table}_{NamingConvention.ForeignKeySuffix}";
        }
    }

    public static class ImportRows
    {
        public const string Table = "import_rows";

        public static class Properties
        {
            public const string Id = "id";
            public const string DocumentId = "document_id";
            public const string SourceId = "source_id";
            public const string Name = "name";
            public const string Description = "description";
            public const string FirstStartAt = "first_start_at";
            public const string CreatedAt = "created_at";
            public const string UpdatedAt = "updated_at";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string DocumentsForeignKey = $"{Table}_{ImportDocuments.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string DocumentOrderIndex = $"{Table}_{Properties.DocumentId}_{Properties.FirstStartAt}_{Properties.Id}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class ImportRowPeriods
    {
        public const string Table = "import_row_periods";

        public static class Properties
        {
            public const string RowId = "row_id";
            public const string Position = "position";
            public const string Description = "description";
            public const string StartAt = "start_at";
            public const string EndAt = "end_at";
            public const string TimeZoneOffsetTicks = "time_zone_offset_ticks";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RowsForeignKey = $"{Table}_{ImportRows.Table}_{NamingConvention.ForeignKeySuffix}";
            public const string TimePeriodCheck = $"{Table}_{Properties.StartAt}_{Properties.EndAt}_{NamingConvention.CheckSuffix}";
            public const string TimePeriodIndex = $"{Table}_{Properties.StartAt}_{Properties.EndAt}_{NamingConvention.IndexSuffix}";
        }
    }

    public static class ImportRowActivities
    {
        public const string Table = "import_row_activities";

        public static class Properties
        {
            public const string RowId = "row_id";
            public const string SourceActivityId = "source_activity_id";
            public const string Position = "position";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RowsForeignKey = $"{Table}_{ImportRows.Table}_{NamingConvention.ForeignKeySuffix}";
        }
    }

    public static class ImportRowTags
    {
        public const string Table = "import_row_tags";

        public static class Properties
        {
            public const string RowId = "row_id";
            public const string SourceTagId = "source_tag_id";
            public const string Position = "position";
        }

        public static class Constraints
        {
            public const string PrimaryKey = $"{Table}_{NamingConvention.PrimaryKeySuffix}";
            public const string RowsForeignKey = $"{Table}_{ImportRows.Table}_{NamingConvention.ForeignKeySuffix}";
        }
    }
}
