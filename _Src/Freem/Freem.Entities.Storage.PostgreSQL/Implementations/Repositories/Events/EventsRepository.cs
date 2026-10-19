using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

internal sealed class EventsRepository : IEventRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly EventEntityToDatabaseEntityConverter _dbConverter;
    private readonly DatabaseEntityToEventEntityConverter _entityConverter;

    public EventsRepository(
        DatabaseContext database, 
        DatabaseContextWriteExceptionHandler exceptionHandler,
        EventEntityToDatabaseEntityConverter dbConverter, 
        DatabaseEntityToEventEntityConverter entityConverter)
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

    public async Task CreateAsync(
        IEntityEvent<Id, UserId> entity, 
        CancellationToken cancellationToken = default) 
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = _dbConverter.Convert(entity);
        
        await _database.Events.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.Handle(context, () => _database.SaveChangesAsync(cancellationToken));
    }
    
    public async Task<SearchEntityResult<IEntityEvent<Id, UserId>>> FindByIdAsync(
        EventId id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Events.FindAsync(e => e.Id == idString, _entityConverter.Convert, cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<IEntityEvent<Id, UserId>>> FindAsync(
        EventAfterTimeFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return await _database.Events
            .Where(e => filter.After == null || e.CreatedAt > filter.After)
            .OrderBy(e => e.CreatedAt)
            .SliceByLimitFilter(filter)
            .CountAndMapAsync(_entityConverter.Convert, cancellationToken);
    }
}