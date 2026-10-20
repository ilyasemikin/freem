using Freem.Domain.Entities.Activities;
using Freem.Domain.Tags;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Activity entity,
        CancellationToken cancellationToken = default)
    {
        var entityId = entity.Id.ToString();
        await context.UpdateRelatedEntitiesAsync<TagId, ActivityTagRelationEntity>(
            entity.Tags,
            e => e.ActivityId == entityId,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => ActivityRelationFactory.CreateDatabaseActivityTagRelation(entity.Id, id),
            cancellationToken);
    }
}