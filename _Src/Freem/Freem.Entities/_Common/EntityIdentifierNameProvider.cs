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

public sealed class EntityIdentifierNameProvider
{
    public static EntityIdentifierNameProvider Instance { get; } = new();
    
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