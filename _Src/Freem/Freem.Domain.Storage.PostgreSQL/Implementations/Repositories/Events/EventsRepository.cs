using Freem.Domain.Entities.Events;
using Freem.Domain.Events;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;
using Freem.Domain.Storage.Repositories.Events;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events;

internal sealed class EventsRepository : IEventRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly EventEnvelopeToDatabaseEntityConverter _dbConverter;
    private readonly DatabaseEntityToEventConverter _entityConverter;

    public EventsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        EventEnvelopeToDatabaseEntityConverter dbConverter,
        DatabaseEntityToEventConverter entityConverter)
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

    public async Task<Result<Error>> CreateAsync(
        EventEnvelope envelope,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(envelope);

        var dbEntity = _dbConverter.Convert(envelope);

        await _database.Events.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(envelope.Event.Id);
        return await _exceptionHandler.Handle(context, () => _database.SaveChangesAsync(cancellationToken));
    }

    public async Task<Result<Error>> CreateManyAsync(
        IReadOnlyList<EventEnvelope> envelopes,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(envelopes);

        if (envelopes.Count == 0)
            return Result<Error>.CreateSuccess();

        var dbEntities = envelopes
            .Select(_dbConverter.Convert)
            .ToList();

        await _database.Events.AddRangeAsync(dbEntities, cancellationToken);

        var context = new DatabaseContextWriteContext(envelopes[0].Event.Id);
        return await _exceptionHandler.Handle(context, () => _database.SaveChangesAsync(cancellationToken));
    }

    public async Task<Result<Event, Error>> FindByIdAsync(
        EventId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Events.FindAsync(
            () => DomainErrorCatalog.Factories.CreateEventNotFound(id),
            e => e.Id == idString,
            _entityConverter.Convert,
            cancellationToken);
    }

    public async Task<Result<AsyncBatch<Event>, Error>> FindAsync(
        EventByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        var entityTypeName = filter.EntityType is null
            ? null
            : EntityTypeNames.ToName(filter.EntityType.Value);

        return await _database.Events
            .Where(e => e.UserId == userIdString)
            .Where(e => entityTypeName == null || e.EntityType == entityTypeName)
            .Where(e => filter.EntityRef == null || e.EntityRef == filter.EntityRef)
            .Where(e => filter.From == null || e.OccurredAt >= filter.From)
            .Where(e => filter.To == null || e.OccurredAt <= filter.To)
            .OrderByDescending(e => e.OccurredAt)
            .ThenByDescending(e => e.Id)
            .AsNoTracking()
            .CountAndMapAsync(filter, _entityConverter.Convert, cancellationToken);
    }
}
