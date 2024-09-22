using Freem.Entities.Records.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

public sealed class RecordAndUserIdentifiers
{
    public RecordIdentifier RecordId { get; }
    public UserIdentifier UserId { get; }
    
    public RecordAndUserIdentifiers(RecordIdentifier recordId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(userId);
        
        RecordId = recordId;
        UserId = userId;
    }
}