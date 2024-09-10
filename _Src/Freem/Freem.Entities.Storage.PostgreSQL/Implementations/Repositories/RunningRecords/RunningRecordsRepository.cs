﻿using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal sealed class RunningRecordsRepository : IRunningRecordRepository
{
    private readonly DatabaseContext _context;
    private readonly DatabaseContextExceptionHandler _exceptionHandler;
    private readonly IEventEntityFactory<RunningRecordEvent, RunningRecord> _eventFactory;

    public RunningRecordsRepository(
        DatabaseContext context,
        DatabaseContextExceptionHandler exceptionHandler,
        IEventEntityFactory<RunningRecordEvent, RunningRecord> eventFactory)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(eventFactory);
        
        _context = context;
        _exceptionHandler = exceptionHandler;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbActivityRelations = entity.CreateDatabaseRunningRecordActivityRelations();
        var dbTagRelations = entity.CreateDatabaseRunningRecordTagRelations();

        await _context.RunningRecords.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbActivityRelations, cancellationToken);
        await _context.AddRangeAsync(dbTagRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task UpdateAsync(RunningRecord entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.RunningRecords.FirstOrDefaultAsync(
            e => e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Description = entity.Description;

        dbEntity.StartAt = entity.StartAt;

        await _context.UpdateRelatedActivitiesAsync(entity, cancellationToken);
        await _context.UpdateRelatedTagsAsync(entity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task RemoveAsync(UserIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.RunningRecords.FirstOrDefaultAsync(e => e.UserId == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _exceptionHandler.HandleSaveChangesAsync(_context, cancellationToken);
    }

    public async Task<SearchEntityResult<RunningRecord>> FindByUserIdAsync(
        UserIdentifier userId,
        CancellationToken cancellationToken)
    {
        return await _context.RunningRecords
            .Include(e => e.Activities)
            .Include(e => e.Tags)
            .FindAsync(e => e.UserId == userId.Value, RunningRecordMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(RunningRecord entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.AddAsync(dbEventEntity, cancellationToken);
    }
}