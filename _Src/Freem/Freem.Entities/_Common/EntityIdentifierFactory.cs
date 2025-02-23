using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.RunningRecords;
using Freem.Entities.Tags;
using Freem.Entities.Users;

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