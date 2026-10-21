using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities.Extensions;
using Utilities.Sorting.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities;

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

    public async Task<Result<Error>> CreateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.CreateDatabaseActivityTagRelations();

        await _database.Activities.AddAsync(dbEntity, cancellationToken);
        await _database.AddRangeAsync(dbRelations, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(Activity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Activities.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return Result<Error>.CreateSuccess();
        
        dbEntity.Name = entity.Name.ToString();
        dbEntity.Status = entity.Status.MapToDatabaseEntityStatus();

        await _database.UpdateRelatedTagsAsync(entity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(ActivityId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _database.Activities.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Activity, Error>> FindByIdAsync(
        ActivityId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var idString = id.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .FindAsync(id, e => e.Id == idString, ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<Activity, Error>> FindByMultipleIdAsync(
        ActivityAndUserIds ids, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        var activityIdString = ids.ActivityId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .FindAsync(
                ids.ActivityId,
                e => e.Id == activityIdString && e.UserId == userIdString,
                ActivityMapper.MapToDomainEntity, 
                cancellationToken);
    }

    public async Task<Result<AsyncBatch<Activity>, Error>> FindAsync(
        ActivityByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        var userIdString = filter.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString)
            .OrderBy(filter.Sorting, ActivityFactories.CreateSortSelector)
            .CountAndMapAsync(filter, ActivityMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<AsyncBatch<Activity>, Error>> FindAsync(ActivityFilter filter, CancellationToken cancellationToken = default)
    {
        var userIdString = filter.UserId.ToString();
        return await _database.Activities
            .Include(e => e.Tags)
            .Where(e => e.UserId == userIdString && e.Name.Contains(filter.SearchText))
            .CountAndMapAsync(filter, ActivityMapper.MapToDomainEntity, cancellationToken);
    }
}