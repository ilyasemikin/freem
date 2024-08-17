using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Relations.Collections;
using DatabaseCategoryStatus = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.CategoryStatus;
using EntityCategoryStatus = Freem.Entities.CategoryStatus;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal static class CategoryMapper
{
    public static CategoryEntity MapToDatabaseEntity(this Category category)
    {
        return new CategoryEntity
        {
            Id = category.Id.Value,
            Name = category.Name,
            Status = MapToDatabaseStatus(category.Status),
            UserId = category.UserId.Value
        };
    }

    public static IEnumerable<CategoryTagRelationEntity> MapToCategoryTagRelations(this Category category)
    {
        return category.Tags.Identifiers.Select(id => MapToCategoryTagRelation(category.Id, id.Value));
    }

    public static CategoryTagRelationEntity MapToCategoryTagRelation(CategoryIdentifier categoryId, string tagIdString)
    {
        return new CategoryTagRelationEntity
        {
            CategoryId = categoryId.Value,
            TagId = tagIdString
        };
    }

    public static DatabaseCategoryStatus MapToDatabaseStatus(this EntityCategoryStatus status)
    {
        return status switch
        {
            EntityCategoryStatus.Active => DatabaseCategoryStatus.Active,
            EntityCategoryStatus.Archived => DatabaseCategoryStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static EntityCategoryStatus MapToDomainEntityStatus(this DatabaseCategoryStatus status)
    {
        return status switch
        {
            DatabaseCategoryStatus.Active => EntityCategoryStatus.Active,
            DatabaseCategoryStatus.Archived => EntityCategoryStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static RelatedTagsCollection MapToDomainRelatedTags(this CategoryEntity entity)
    {
        var tags = Enumerable.Empty<Tag>();
        if (entity.Tags is not null)
            tags = entity.Tags.Select(TagMapper.MapToDomainEntity);

        return new RelatedTagsCollection(tags);
    }
    
    public static Category MapToDomainEntity(this CategoryEntity entity)
    {
        var id = new CategoryIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var tags = MapToDomainRelatedTags(entity);
        return new Category(id, userId, tags, MapToDomainEntityStatus(entity.Status));
    }
}
