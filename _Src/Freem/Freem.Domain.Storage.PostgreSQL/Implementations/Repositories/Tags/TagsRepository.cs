using Freem.Domain;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Tags;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Sorting.Extensions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Tags;

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

    public async Task<Result<Error>> CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = entity.MapToDatabaseEntity();

        await _database.Tags.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Tags.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(actualEntity, entity))
            return Result<Error>.CreateSuccess();

        dbEntity.Name = entity.Name.ToString();

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(TagId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var dbEntity = await _database.Tags.FindEntityAsync(id, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);

        _database.Remove(dbEntity);

        var context = new DatabaseContextWriteContext(id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Tag, Error>> FindByIdAsync(
        TagId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Tags.FindAsync(id, e => e.Id == idString, TagMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<Tag, Error>> FindByMultipleIdAsync(
        TagAndUserIds ids,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);

        var tagIdString = ids.TagId.ToString();
        var userIdString = ids.UserId.ToString();
        return await _database.Tags.FindAsync(
            ids.TagId,
            e => e.Id == tagIdString && e.UserId == userIdString,
            TagMapper.MapToDomainEntity,
            cancellationToken);
    }

    public async Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagByUserFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var userIdString = filter.UserId.ToString();
        return await _database.Tags
            .Where(e => e.UserId == userIdString)
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .CountAndMapAsync(filter, TagMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<Result<AsyncBatch<Tag>, Error>> FindAsync(
        TagFilter filter,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        var userIdString = filter.UserId.ToString();
        return await _database.Tags
            .Where(e => e.UserId == userIdString && e.Name.Contains(filter.SearchText))
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .CountAndMapAsync(filter, TagMapper.MapToDomainEntity, cancellationToken);
    }
}