using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tags;

public sealed class Tag : 
    IEntity<TagIdentifier>,
    ICloneable<Tag>
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

    public Tag Clone()
    {
        return new Tag(Id, UserId, Name);
    }
}
