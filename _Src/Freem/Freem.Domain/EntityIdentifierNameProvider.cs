using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain;

public sealed class EntityIdentifierNameProvider
{
    public static EntityIdentifierNameProvider Instance { get; } = new();
    
    public string Get(IEntityIdentifier identifier)
    {
        return identifier switch
        {
            ActivityIdentifier => EntityNames.Activity,
            RecordIdentifier => EntityNames.Record,
            RunningRecordIdentifier => EntityNames.RunningRecord,
            TagIdentifier => EntityNames.Tag,
            UserIdentifier => EntityNames.User,
            _ => throw new ArgumentOutOfRangeException(nameof(identifier), identifier.GetType(), string.Empty)
        };
    }
}