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
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories.Extensions;
using Freem.Sorting.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal sealed class CategoriesRepository : ICategoriesRepository
{
    private readonly DatabaseContext _context;
    private readonly IEventEntityFactory<CategoryEvent, Category> _eventFactory;

    public CategoriesRepository(
        DatabaseContext context,
        IEventEntityFactory<CategoryEvent, Category> eventFactory)
    {
        _context = context;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.CreateDatabaseCategoryTagRelations();

        await _context.Categories.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _context.Categories.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);
        
        dbEntity.Name = entity.Name;
        dbEntity.Status = entity.Status.MapToDatabaseEntityStatus();

        await _context.UpdateRelatedTagsAsync(entity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(CategoryIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var dbEntity = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Category>> FindByIdAsync(
        CategoryIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _context.Categories
            .Include(e => e.Tags)
            .FindAsync(e => e.Id == id.Value, CategoryMapper.MapToDomainEntity, cancellationToken);
    }

    public async Task<SearchEntityResult<Category>> FindAsync(CategoryAndUserIdentifiers ids, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);
        
        return await _context.Categories
            .Include(e => e.Tags)
            .FindAsync(
                e => e.Id == ids.CategoryId.Value && e.UserId == ids.UserId.Value,
                CategoryMapper.MapToDomainEntity, 
                cancellationToken);
    }

    public async Task<SearchEntitiesAsyncResult<Category>> FindByUserAsync(CategoriesByUserFilter filter, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        
        return await _context.Categories
            .Where(e => e.UserId == filter.UserId.Value)
            .OrderBy(filter.Sorting, CategoryFactories.CreateSortSelector)
            .SliceByLimitAndOffsetFilter(filter)
            .CountAndMapAsync(CategoryMapper.MapToDomainEntity, cancellationToken);
    }

    private async Task WriteEventAsync(Category entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}