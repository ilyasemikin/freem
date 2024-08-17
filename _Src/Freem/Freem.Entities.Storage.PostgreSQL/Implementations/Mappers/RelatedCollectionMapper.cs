using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

internal static class RelatedCollectionMapper
{
    public static RelatedTagsCollection MapToRelatedTagsCollection(this IEnumerable<TagEntity>? entities)
    {
        var tags = Enumerable.Empty<Tag>();
        if (entities is not null)
            tags = entities.Select(TagMapper.MapToDomainEntity);

        return new RelatedTagsCollection(tags);
    }

    public static RelatedCategoriesCollection MapToRelatedCategoriesCollection(this IEnumerable<CategoryEntity>? entities)
    {
        var categories = Enumerable.Empty<Category>();
        if (entities is not null)
            categories = entities.Select(CategoryMapper.MapToDomainEntity);

        return new RelatedCategoriesCollection(categories);
    }
}
