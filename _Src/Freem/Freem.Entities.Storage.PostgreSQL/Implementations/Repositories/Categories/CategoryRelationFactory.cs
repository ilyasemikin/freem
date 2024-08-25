using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal static class CategoryRelationFactory
{
    public static IEnumerable<CategoryTagRelationEntity> CreateDatabaseCategoryTagRelations(this Category category)
    {
        return category.Tags.Identifiers.Select(id => CreateDatabaseCategoryTagRelation(category.Id, id.Value));
    }

    public static CategoryTagRelationEntity CreateDatabaseCategoryTagRelation(CategoryIdentifier categoryId, string tagIdString)
    {
        return new CategoryTagRelationEntity
        {
            CategoryId = categoryId.Value,
            TagId = tagIdString
        };
    }
}