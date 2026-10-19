using Freem.Domain.Entities.Tags;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal static class TagMapper
{
    public static TagEntity MapToDatabaseEntity(this Tag tag)
    {
        return new TagEntity
        {
            Id = tag.Id.ToString(),
            Name = tag.Name.ToString(),
            UserId = tag.UserId.ToString(),
        };
    }

    public static Tag MapToDomainEntity(this TagEntity entity)
    {
        var id = TagId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);
        var name = TagName.Parse(entity.Name, null);
        return new Tag(id, userId, name);
    }
}
