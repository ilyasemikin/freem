using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.RunningRecords;
using Freem.Entities.Tags;
using Freem.Entities.Users;

namespace Freem.Entities;

public sealed class EntityIdentifierNameProvider
{
    public string Get(IEntityIdentifier identifier)
    {
        return identifier switch
        {
            ActivityIdentifier => Activity.EntityName,
            RecordIdentifier => Record.EntityName,
            RunningRecordIdentifier => RunningRecord.EntityName,
            TagIdentifier => Tag.EntityName,
            UserIdentifier => User.EntityName,
            _ => throw new ArgumentOutOfRangeException(nameof(identifier), identifier.GetType(), string.Empty)
        };
    }
}