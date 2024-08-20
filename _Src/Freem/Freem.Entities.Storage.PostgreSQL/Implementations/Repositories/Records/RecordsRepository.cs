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

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal sealed class RecordsRepository : IRecordsRepository
{
    private readonly DatabaseContext _context;

    private readonly IEventEntityFactory<RecordEvent, EventIdentifier, UserIdentifier, Record, RecordIdentifier>
        _eventFactory;

    public RecordsRepository(DatabaseContext context,
        IEventEntityFactory<RecordEvent, EventIdentifier, UserIdentifier, Record, RecordIdentifier> eventFactory)
    {
        _context = context;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbCategoryRelations = entity.MapToRecordCategoryRelations();
        var dbTagRelations = entity.MapToRecordTagRelations();

        await _context.Records.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbCategoryRelations, cancellationToken);
        await _context.AddRangeAsync(dbTagRelations, cancellationToken);

        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Record entity, CancellationToken cancellationToken = default)
    {
        static async Task UpdateCategoryRelationsAsync(DatabaseContext context, Record entity,
            CancellationToken cancellationToken)
        {
            var currentCategoryIds = context.FindRelatedIds<RecordCategoryRelationEntity>(
                e => e.RecordId == entity.Id.Value,
                e => e.CategoryId);

            var (categoryIdsToRemove, categoryIdsToAdd) =
                currentCategoryIds.ExceptMutual(entity.Categories.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RecordCategoryRelationEntity>(
                e => categoryIdsToRemove.Contains(e.CategoryId),
                cancellationToken);

            var newDbCategoryRelations =
                categoryIdsToAdd.Select(id => RecordMapper.MapToRecordCategoryRelation(entity.Id, id));
            await context.AddRangeAsync(newDbCategoryRelations, cancellationToken);
        }

        static async Task UpdateTagRelationsAsync(DatabaseContext context, Record entity,
            CancellationToken cancellationToken)
        {
            var currentTagIds = context.FindRelatedIds<RecordTagRelationEntity>(
                e => e.RecordId == entity.Id.Value,
                e => e.TagId);

            var (tagIdsToRemove, tagIdsToAdd) = currentTagIds.ExceptMutual(entity.Tags.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RecordTagRelationEntity>(
                e => tagIdsToRemove.Contains(e.TagId),
                cancellationToken);

            var newDbTagsRelations = tagIdsToAdd.Select(id => RecordMapper.MapToRecordTagRelation(entity.Id, id));
            await context.AddRangeAsync(newDbTagsRelations, cancellationToken);
        }

        var dbEntity = await _context.Records.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.Period.StartAt;
        dbEntity.EndAt = entity.Period.EndAt;

        await UpdateCategoryRelationsAsync(_context, entity, cancellationToken);
        await UpdateTagRelationsAsync(_context, entity, cancellationToken);

        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Records.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Record>> FindByIdAsync(RecordIdentifier id,
        CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Records
            .Include(e => e.Categories)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            return SearchEntityResult<Record>.NotFound();

        var entity = dbEntity.MapToDomainEntity();
        return SearchEntityResult<Record>.Found(entity);
    }

    private async Task WriteEventAsync(Record entity, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}