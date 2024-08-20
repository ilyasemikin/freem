using Freem.Collections.Extensions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Abstractions.Identifiers.Extensions;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal sealed class RunningRecordsRepository : IRunningRecordRepository
{
    private readonly DatabaseContext _context;

    private readonly IEventEntityFactory<
            RunningRecordEvent,
            EventIdentifier,
            UserIdentifier,
            RunningRecord,
            UserIdentifier>
        _eventFactory;

    public RunningRecordsRepository(
        DatabaseContext context,
        IEventEntityFactory<RunningRecordEvent, EventIdentifier, UserIdentifier, RunningRecord, UserIdentifier>
            eventFactory)
    {
        _context = context;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbCategoryRelations = entity.MapToRunningRecordCategoryRelations();
        var dbTagRelations = entity.MapToRunningRecordTagRelations();

        await _context.RunningRecords.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbCategoryRelations, cancellationToken);
        await _context.AddRangeAsync(dbTagRelations, cancellationToken);

        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        static async Task UpdateCategoryRelationsAsync(DatabaseContext context, RunningRecord entity,
            CancellationToken cancellationToken)
        {
            var currentCategoryIds = context.FindRelatedIds<RunningRecordCategoryRelationEntity>(
                e => e.RunningRecordUserId == entity.Id.Value,
                e => e.CategoryId);

            var (categoryIdsToRemove, categoryIdsToAdd) =
                currentCategoryIds.ExceptMutual(entity.Categories.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RunningRecordCategoryRelationEntity>(
                e => categoryIdsToRemove.Contains(e.RunningRecordUserId),
                cancellationToken);

            var newDbCategoryRelations =
                categoryIdsToAdd.Select(id => RunningRecordMapper.MapToRunningRecordTagRelation(entity.Id, id));
            await context.AddRangeAsync(newDbCategoryRelations, cancellationToken);
        }

        static async Task UpdateTagRelationsAsync(DatabaseContext context, RunningRecord entity,
            CancellationToken cancellationToken)
        {
            var currentTagIds = context.FindRelatedIds<RunningRecordTagRelationEntity>(
                e => e.RunningRecordUserId == entity.Id.Value,
                e => e.TagId);

            var (tagIdsToRemove, tagIdsToAdd) = currentTagIds.ExceptMutual(entity.Tags.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RunningRecordTagRelationEntity>(
                e => tagIdsToRemove.Contains(e.TagId),
                cancellationToken);

            var newDbTagsRelations =
                tagIdsToAdd.Select(id => RunningRecordMapper.MapToRunningRecordTagRelation(entity.Id, id));
            await context.AddRangeAsync(newDbTagsRelations, cancellationToken);
        }

        var dbEntity = await _context.RunningRecords.FirstOrDefaultAsync(
            e => e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.StartAt;

        await UpdateCategoryRelationsAsync(_context, entity, cancellationToken);
        await UpdateTagRelationsAsync(_context, entity, cancellationToken);

        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(UserIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.RunningRecords.FirstOrDefaultAsync(e => e.UserId == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(UserIdentifier userId,
        CancellationToken cancellationToken)
    {
        var dbEntity = await _context.RunningRecords
            .AsNoTracking()
            .Include(e => e.Categories)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.UserId == userId.Value, cancellationToken);

        if (dbEntity is null)
            return SearchEntityResult<RunningRecord>.NotFound();

        var entity = dbEntity.MapToDomainEntity();
        return SearchEntityResult<RunningRecord>.Found(entity);
    }

    private async Task WriteEventAsync(RunningRecord entity, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.AddAsync(dbEventEntity, cancellationToken);
    }
}