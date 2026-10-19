using Freem.Domain.Events.Identifiers;
using Freem.Domain.Tags.Events.Created;
using Freem.Domain.Tags.Events.Removed;
using Freem.Domain.Tags.Events.Updated;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Entities.Tags;

public sealed class Tag : 
    IEntity<TagIdentifier>
{
    private TagName _name = null!;

    public TagIdentifier Id { get; }
    public UserIdentifier UserId { get; }

    public TagName Name
    {
        get => _name;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            _name = value;
        }
    }

    public Tag(
        TagIdentifier id, 
        UserIdentifier userId, 
        TagName name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }

    public TagCreatedEvent BuildCreatedEvent(EventIdentifier eventId)
    {
        return new TagCreatedEvent(eventId, Id, UserId);
    }

    public TagUpdatedEvent BuildUpdatedEvent(EventIdentifier eventId)
    {
        return new TagUpdatedEvent(eventId, Id, UserId);
    }

    public TagRemovedEvent BuildRemovedEvent(EventIdentifier eventId)
    {
        return new TagRemovedEvent(eventId, Id, UserId);
    }
}
