using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Exceptions;
using Freem.Domain.Storage.Models;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Tags;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Sorting.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal sealed class TagsRepository : ITagRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEqualityComparer<Tag> _equalityComparer;

    public TagsRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEqualityComparer<Tag> equalityComparer)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(equalityComparer);

        _database = database;
        _exceptionHandler = exceptionHandler;
        _equalityComparer = equalityComparer;
    }

    public async Task CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = entity.MapToDatabaseEntity();

        await _database.Tags.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Tags.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(actualEntity, entity))
            return;

        dbEntity.Name = entity.Name.ToString();

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task DeleteAsync(TagId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var dbEntity = await _database.Tags.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<SearchEntityResult<Tag>> FindByIdAsync(
        TagId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Tags.FindAsync(e => e.Id == idString, TagMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntityResult<Tag>> FindByMultipleIdAsync(
        TagAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var tagIdString = ids.TagId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Tags.FindAsync(
            e => e.Id == tagIdString && e.UserId == userIdString,
            TagMapper.MapToDomainEntity,
            cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Tag>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        return await _database.Tags
            .Where(e => e.UserId == userIdString)
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(TagMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Tag>> FindAsync(
        TagFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        var userIdString = filter.UserId.ToString();
        return await _database.Tags
            .Where(e => e.UserId == userIdString && e.Name.Contains(filter.SearchText))
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .SliceByLimitFilter(filter)
            .CountAndMapAsync(TagMapper.MapToDomainEntity, cancellationToken);
    }
}