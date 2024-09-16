using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal static class TagMapper
{
    public static TagEntity MapToDatabaseEntity(this Tag tag)
    {
        return new TagEntity
        {
            Id = tag.Id.Value,
            Name = tag.Name,
            UserId = tag.UserId.Value,
        };
    }

    public static Tag MapToDomainEntity(this TagEntity entity)
    {
        var id = new TagIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        return new Tag(id, userId, entity.Name);
    }
}
