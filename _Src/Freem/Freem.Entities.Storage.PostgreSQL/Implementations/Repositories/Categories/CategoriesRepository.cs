using Freem.Collections.Extensions;
using Freem.Entities.Abstractions.Identifiers.Extensions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal class CategoriesRepository : ICategoriesRepository
{
    private readonly DatabaseContext _context;

    public CategoriesRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.MapToCategoryTagRelations();

        await _context.Categories.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbRelations, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        static async Task UpdateTagRelationsAsync(DatabaseContext context, Category entity, CancellationToken cancellationToken)
        {
            var currentTagIds = context.FindRelatedIds<CategoryTagRelationEntity>(
            e => e.CategoryId == entity.Id.Value,
            e => e.TagId);

            var (tagIdsToRemove, tagIdsToAdd) = currentTagIds.ExceptMutual(entity.Tags.Identifiers.AsValues());
            await context.RemoveRelationsAsync<CategoryTagRelationEntity>(
                e => tagIdsToRemove.Contains(e.TagId),
                cancellationToken);

            var newDbRelations = tagIdsToAdd.Select(id => CategoryMapper.MapToCategoryTagRelation(entity.Id, id));
            await context.AddRangeAsync(newDbRelations, cancellationToken);
        }

        var dbEntity = await _context.Categories.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.UserId.Value, 
            cancellationToken);        
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;
        dbEntity.Status = entity.Status.MapToDatabaseStatus();

        await UpdateTagRelationsAsync(_context, entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(CategoryIdentifier id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Categories
            .Where(e => e.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);

        if (count == 0)
            throw new NotFoundException(id);
    }

    public async Task<int> RemoveMultipleByUserAsync(
        UserIdentifier userId, 
        IEnumerable<CategoryIdentifier> ids, 
        CancellationToken cancellationToken = default)
    {
        var idsStrings = ids.AsValues();
        return await _context.Categories
            .Where(e => e.UserId == userId.Value && idsStrings.Contains(e.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Category>> FindByIdAsync(CategoryIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Categories
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            return SearchEntityResult<Category>.NotFound();
        
        var entity = dbEntity.MapToDomainEntity();
        return SearchEntityResult<Category>.Found(entity);
    }
}
