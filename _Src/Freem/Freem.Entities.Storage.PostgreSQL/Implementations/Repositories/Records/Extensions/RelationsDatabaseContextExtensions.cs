using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Entities.Records;
using Freem.Domain.Tags.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedActivitiesAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<ActivityIdentifier, RecordActivityRelationEntity>(
            entity.Activities,
            e => e.RecordId == entity.Id,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId) && e.RecordId == entity.Id,
            id => RecordRelationFactory.CreateDatabaseRecordActivityRelation(entity.Id, id),
            cancellationToken);
    }
    
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<TagIdentifier, RecordTagRelationEntity>(
            entity.Tags,
            e => e.RecordId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId) && e.RecordId == entity.Id,
            id => RecordRelationFactory.CreateDatabaseRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}