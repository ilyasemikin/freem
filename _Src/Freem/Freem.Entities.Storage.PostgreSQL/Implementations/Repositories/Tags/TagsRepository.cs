using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Tags.Models;
using Freem.Sorting.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal sealed class TagsRepository : ITagsRepository
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

        dbEntity.Name = entity.Name;

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task DeleteAsync(TagIdentifier id, CancellationToken cancellationToken = default)
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
        TagIdentifier id, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _database.Tags.FindAsync(e => e.Id == id, TagMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Tag>> FindByMultipleIdAsync(
        TagAndUserIdentifiers ids, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _database.Tags.FindAsync(
            e => e.Id == ids.TagId && e.UserId == ids.UserId,
            TagMapper.MapToDomainEntity, 
            cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Tag>> FindAsync(
        TagsByUserFilter filter, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _database.Tags
            .Where(e => e.UserId == filter.UserId)
            .OrderBy(filter.Sorting, TagFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(TagMapper.MapToDomainEntity, cancellationToken);
    }
    
    public async Task<SearchEntityResult<Tag>> FindByNameAsync(
        TagName name, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        return await _database.Tags.FindAsync(
            e => e.Name == (string)name,
            TagMapper.MapToDomainEntity,
            cancellationToken);
    }
}
