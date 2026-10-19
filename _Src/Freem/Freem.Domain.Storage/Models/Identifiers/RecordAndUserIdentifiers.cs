using Freem.Domain.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Storage.Models.Identifiers;

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