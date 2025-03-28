﻿using Freem.Entities.Activities;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Activity entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, ActivityTagRelationEntity>(
            entity,
            e => e.ActivityId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => ActivityRelationFactory.CreateDatabaseActivityTagRelation(entity.Id, id),
            cancellationToken);
    }
}