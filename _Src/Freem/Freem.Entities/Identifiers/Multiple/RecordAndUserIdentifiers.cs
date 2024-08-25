namespace Freem.Entities.Identifiers.Multiple;

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