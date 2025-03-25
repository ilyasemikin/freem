using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Constants;

public static class TriggerErrorParameters
{
    public const string ActivityId = "activity_id";
    public const string RecordId = "record_id";
    public const string TagId = "tag_id";
    public const string UserId = "user_id";

    public const string EventEntityName = EntitiesNames.Events.Properties.EntityName;
    public const string EventEntityId = EntitiesNames.Events.Properties.EntityId;
    
    public const string ActualCount = "actual_count";
}