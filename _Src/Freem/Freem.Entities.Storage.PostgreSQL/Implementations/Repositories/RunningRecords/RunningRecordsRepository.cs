using Freem.Collections.Extensions;
using Freem.Entities.Abstractions.Identifiers.Extensions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal class RunningRecordsRepository : IRunningRecordRepository
{
    private readonly DatabaseContext _context;

    public RunningRecordsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbCategoryRelations = entity.MapToRunningRecordCategoryRelations();
        var dbTagRelations = entity.MapToRunningRecordTagRelations();

        await _context.RunningRecords.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbCategoryRelations, cancellationToken);
        await _context.AddRangeAsync(dbTagRelations, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        static async Task UpdateCategoryRelationsAsync(DatabaseContext context, RunningRecord entity, CancellationToken cancellationToken)
        {
            var currentCategoryIds = context.FindRelatedIds<RunningRecordCategoryRelationEntity>(
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.CategoryId);

            var (categoryIdsToRemove, categoryIdsToAdd) = currentCategoryIds.ExceptMutual(entity.Categories.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RunningRecordCategoryRelationEntity>(
                e => currentCategoryIds.Contains(e.RunningRecordUserId),
                cancellationToken);

            var newDbCategoryRelations = categoryIdsToAdd.Select(id => RunningRecordMapper.MapToRunningRecordTagRelation(entity.Id, id));
            await context.AddRangeAsync(newDbCategoryRelations, cancellationToken);
        }

        static async Task UpdateTagRelationsAsync(DatabaseContext context, RunningRecord entity, CancellationToken cancellationToken)
        {
            var currentTagIds = context.FindRelatedIds<RunningRecordTagRelationEntity>(
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.TagId);

            var (tagIdsToRemove, tagIdsToAdd) = currentTagIds.ExceptMutual(entity.Tags.Identifiers.AsValues());
            await context.RemoveRelationsAsync<RunningRecordTagRelationEntity>(
                e => tagIdsToRemove.Contains(e.TagId),
                cancellationToken);

            var newDbTagsRelations = tagIdsToAdd.Select(id => RunningRecordMapper.MapToRunningRecordTagRelation(entity.Id, id));
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

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(UserIdentifier id, CancellationToken cancellationToken = default)
    {
        var count = await _context.RunningRecords
            .Where(e => e.UserId == id.Value)
            .ExecuteDeleteAsync(cancellationToken);

        if (count == 0)
            throw new NotFoundException(id);
    }

    public async Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(UserIdentifier userId, CancellationToken cancellationToken)
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
}
