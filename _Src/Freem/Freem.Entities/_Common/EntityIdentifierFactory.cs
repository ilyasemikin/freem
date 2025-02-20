using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities;

public sealed class EntityIdentifierFactory
{
    public IEntityIdentifier Create(string entity, string value)
    {
        return entity switch
        {
            Activity.EntityName => new ActivityIdentifier(value),
            Record.EntityName => new RecordIdentifier(value),
            RunningRecord.EntityName => new RunningRecordIdentifier(value),
            Tag.EntityName => new TagIdentifier(value),
            User.EntityName => new UserIdentifier(value),
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, string.Empty)
        };
    }
}