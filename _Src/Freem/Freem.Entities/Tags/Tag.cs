using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Tags.Events.Created;
using Freem.Entities.Tags.Events.Removed;
using Freem.Entities.Tags.Events.Updated;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tags;

public sealed class Tag : 
    IEntity<TagIdentifier>,
    ICloneable<Tag>
{
    public const string EntityName = "tag";
    
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

    public Tag Clone()
    {
        return new Tag(Id, UserId, Name);
    }
}
