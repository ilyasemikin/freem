using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Users;

namespace Freem.Domain;

public sealed class EntityIdFactory
{
    public static EntityIdFactory Instance { get; } = new();
    
    public Id Create(string entity, string value)
    {
        return entity switch
        {
            EntityNames.Activity => ActivityId.Parse(value, null),
            EntityNames.Record => RecordId.Parse(value, null),
            EntityNames.RunningRecord => RunningRecordId.Parse(value, null),
            EntityNames.Tag => TagId.Parse(value, null),
            EntityNames.User => UserId.Parse(value, null),
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, string.Empty)
        };
    }
}