using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;
using Freem.Sorting.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal sealed class ActivitiesRepository : IActivityRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEqualityComparer<Activity> _equalityComparer;

    public ActivitiesRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEqualityComparer<Activity> equalityComparer)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _equalityComparer = equalityComparer;
    }

    public async Task CreateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.CreateDatabaseActivityTagRelations();

        await _database.Activities.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbRelations, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Activities.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return;
        
        dbEntity.Name = entity.Name.ToString();
        dbEntity.Status = entity.Status.MapToDatabaseEntityStatus();

        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task DeleteAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _database.Activities.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<SearchEntityResult<Activity>> FindByIdAsync(
        ActivityId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var idString = id.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .FindAsync(e => e.Id == idString, ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntityResult<Activity>> FindByMultipleIdAsync(
        ActivityAndUserIds ids, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        var activityIdString = ids.ActivityId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .FindAsync(
                e => e.Id == activityIdString && e.UserId == userIdString,
                ActivityMapper.MapToDomainEntity, 
                cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Activity>> FindAsync(
        ActivityByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        var userIdString = filter.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString)
            .OrderBy(filter.Sorting, ActivityFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Activity>> FindAsync(ActivityFilter filter, CancellationToken cancellationToken = default)
    {
        var userIdString = filter.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString && e.Name.Contains(filter.SearchText))
            .SliceByLimitFilter(filter)
            .CountAndMapAsync(ActivityMapper.MapToDomainEntity, cancellationToken);
    }
}