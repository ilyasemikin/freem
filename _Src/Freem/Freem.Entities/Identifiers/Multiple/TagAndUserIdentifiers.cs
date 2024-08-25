namespace Freem.Entities.Identifiers.Multiple;

public sealed class TagAndUserIdentifiers
{
    public TagIdentifier TagId { get; }
    public UserIdentifier UserId { get; }
    
    public TagAndUserIdentifiers(TagIdentifier tagId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(tagId);
        ArgumentNullException.ThrowIfNull(userId);
        
        TagId = tagId;
        UserId = userId;
    }
}