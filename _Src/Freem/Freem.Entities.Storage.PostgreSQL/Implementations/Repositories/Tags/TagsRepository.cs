using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Identifiers.Multiple;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Sorting.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal sealed class TagsRepository : ITagsRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEventEntityFactory<TagEvent, Tag> _eventFactory;

    public TagsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEventEntityFactory<TagEvent, Tag> eventFactory)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(eventFactory);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();

        await _database.Tags.AddAsync(dbEntity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _database.Tags.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value, 
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);
        
        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task RemoveAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _database.Tags.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<SearchEntityResult<Tag>> FindByIdAsync(
        TagIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _database.Tags.FindAsync(e => e.Id == id.Value, TagMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Tag>> FindAsync(
        TagAndUserIdentifiers ids, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _database.Tags.FindAsync(
            e => e.Id == ids.TagId.Value && e.UserId == ids.UserId.Value,
            TagMapper.MapToDomainEntity, 
            cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Tag>> FindByUserAsync(
        TagsByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _database.Tags
            .Where(e => e.UserId == filter.UserId.Value)
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(TagMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(Tag entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _database.AddAsync(dbEventEntity, cancellationToken);
    }
}
