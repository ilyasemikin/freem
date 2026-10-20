using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Users;

namespace Freem.Domain;

public sealed class EntityIdNameProvider
{
    public static EntityIdNameProvider Instance { get; } = new();
    
    public string Get(Id id)
    {
        return id switch
        {
            ActivityId => EntityNames.Activity,
            RecordId => EntityNames.Record,
            RecordPeriodId => EntityNames.RecordPeriod,
            RunningRecordId => EntityNames.RunningRecord,
            TagId => EntityNames.Tag,
            UserId => EntityNames.User,
            _ => throw new ArgumentOutOfRangeException(nameof(id), id.GetType(), string.Empty)
        };
    }
}