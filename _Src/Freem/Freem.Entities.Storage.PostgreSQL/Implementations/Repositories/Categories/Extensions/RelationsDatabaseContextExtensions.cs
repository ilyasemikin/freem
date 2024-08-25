using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Category entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, CategoryTagRelationEntity>(
            entity,
            e => e.CategoryId == entity.Id.Value,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => CategoryRelationFactory.CreateDatabaseCategoryTagRelation(entity.Id, id),
            cancellationToken);
    }
}