using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain;

public sealed class EntityIdentifierFactory
{
    public static EntityIdentifierFactory Instance { get; } = new();
    
    public IEntityIdentifier Create(string entity, string value)
    {
        return entity switch
        {
            EntityNames.Activity => new ActivityIdentifier(value),
            EntityNames.Record => new RecordIdentifier(value),
            EntityNames.RunningRecord => new RunningRecordIdentifier(value),
            EntityNames.Tag => new TagIdentifier(value),
            EntityNames.User => new UserIdentifier(value),
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, string.Empty)
        };
    }
}