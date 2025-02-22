using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Models.Identifiers;

public sealed class TagAndUserIdentifiers : IMultipleEntityIdentifier
{
    public TagIdentifier TagId { get; }
    public UserIdentifier UserId { get; }

    public IEnumerable<IEntityIdentifier> Ids
    {
        get
        {
            yield return TagId;
            yield return UserId;
        }
    }

    public TagAndUserIdentifiers(TagIdentifier tagId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(tagId);
        ArgumentNullException.ThrowIfNull(userId);
        
        TagId = tagId;
        UserId = userId;
    }
}