using Freem.Domain.Tags;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Models.Ids;

public sealed class TagAndUserIds
{
    public TagId TagId { get; }
    public UserId UserId { get; }

    public TagAndUserIds(TagId tagId, UserId userId)
    {
        ArgumentNullException.ThrowIfNull(tagId);
        ArgumentNullException.ThrowIfNull(userId);
        
        TagId = tagId;
        UserId = userId;
    }
}