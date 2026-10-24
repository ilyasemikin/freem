using Freem.Domain.Events;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Events;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Entities.Tags;

public sealed class Tag : 
    IEntity<TagId>
{
    public TagId Id { get; }
    public UserId UserId { get; }

    public TagName Name
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            field = value;
        }
    }

    public Tag(
        TagId id, 
        UserId userId, 
        TagName name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }

    public TagCreatedEvent BuildCreatedEvent(EventId eventId)
    {
        return new TagCreatedEvent(eventId)
        {
            TagId = Id,
            Name = Name
        };
    }

    public TagUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        return new TagUpdatedEvent(eventId)
        {
            TagId = Id,
            Name = Name
        };
    }

    public TagRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new TagRemovedEvent(eventId)
        {
            TagId = Id
        };
    }
}
