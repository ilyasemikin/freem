using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

public sealed class RecordAndUserIdentifiers : IMultipleEntityIdentifier
{
    public RecordIdentifier RecordId { get; }
    public UserIdentifier UserId { get; }

    public IEnumerable<IEntityIdentifier> Ids
    {
        get
        {
            yield return RecordId;
            yield return UserId;
        }
    }

    public RecordAndUserIdentifiers(RecordIdentifier recordId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(userId);
        
        RecordId = recordId;
        UserId = userId;
    }
}