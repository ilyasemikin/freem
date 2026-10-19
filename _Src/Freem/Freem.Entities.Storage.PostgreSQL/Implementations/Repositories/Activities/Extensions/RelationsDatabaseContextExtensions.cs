using Freem.Domain.Entities.Activities;
using Freem.Domain.Tags.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Activity entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<TagIdentifier, ActivityTagRelationEntity>(
            entity.Tags,
            e => e.ActivityId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => ActivityRelationFactory.CreateDatabaseActivityTagRelation(entity.Id, id),
            cancellationToken);
    }
}