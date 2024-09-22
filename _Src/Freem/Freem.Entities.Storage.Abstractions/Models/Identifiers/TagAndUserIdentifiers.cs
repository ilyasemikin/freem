using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

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