using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Records;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Tags;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedActivitiesAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Activity, ActivityIdentifier, RecordActivityRelationEntity>(
            entity,
            e => e.RecordId == entity.Id,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId),
            id => RecordRelationFactory.CreateDatabaseRecordActivityRelation(entity.Id, id),
            cancellationToken);
    }
    
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, RecordTagRelationEntity>(
            entity,
            e => e.RecordId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => RecordRelationFactory.CreateDatabaseRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}