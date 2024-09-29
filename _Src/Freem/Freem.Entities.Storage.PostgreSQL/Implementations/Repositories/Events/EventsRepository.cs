using System.Text.Json;
using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

internal sealed class EventsRepository : IEventsRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventEntity> _dbConverter;
    private readonly IConverter<EventEntity, IEntityEvent<IEntityIdentifier, UserIdentifier>> _entityConverter;

    public EventsRepository(
        DatabaseContext database, 
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventEntity> dbConverter, 
        IConverter<EventEntity, IEntityEvent<IEntityIdentifier, UserIdentifier>> entityConverter)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(dbConverter);
        ArgumentNullException.ThrowIfNull(entityConverter);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _dbConverter = dbConverter;
        _entityConverter = entityConverter;
    }

    public async Task CreateAsync<TEvent>(TEvent entity, CancellationToken cancellationToken = default) 
        where TEvent : class, IEntityEvent<IEntityIdentifier, UserIdentifier>
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = _dbConverter.Convert(entity);
        
        await _database.Events.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.Handle(context, () => _database.SaveChangesAsync(cancellationToken));
    }

    public async Task<SearchEntityResult<TEvent>> FindByIdAsync<TEvent>(
        EventIdentifier id, 
        CancellationToken cancellationToken = default) 
        where TEvent : class, IEntityEvent<IEntityIdentifier, UserIdentifier>
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _database.Events
            .FindAsync(e => e.Id == id, Convert, cancellationToken);

        TEvent? Convert(EventEntity entity)
        {
            return _entityConverter.Convert(entity) as TEvent;
        }
    }

    public async Task<SearchEntitiesAsyncResult<IEntityEvent<IEntityIdentifier, UserIdentifier>>> FindAfterAsync(
        EventsAfterTimeFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return await _database.Events
            .Where(e => e.CreatedAt > filter.After)
            .OrderBy(e => e.CreatedAt)
            .SliceByLimitFilter(filter)
            .CountAndMapAsync(_entityConverter.Convert, cancellationToken);
    }
}