﻿using Freem.Collections.Extensions;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Abstractions.Identifiers.Extensions;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal sealed class CategoriesRepository : ICategoriesRepository
{
    private readonly DatabaseContext _context;

    private readonly IEventEntityFactory<CategoryEvent, EventIdentifier, UserIdentifier, Category, CategoryIdentifier>
        _eventFactory;

    public CategoriesRepository(
        DatabaseContext context,
        IEventEntityFactory<CategoryEvent, EventIdentifier, UserIdentifier, Category, CategoryIdentifier> eventFactory)
    {
        _context = context;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();
        var dbRelations = entity.MapToCategoryTagRelations();

        await _context.Categories.AddAsync(dbEntity, cancellationToken);
        await _context.AddRangeAsync(dbRelations, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        static async Task UpdateTagRelationsAsync(DatabaseContext context, Category entity,
            CancellationToken cancellationToken)
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

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(CategoryIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        _context.Remove(dbEntity);
        
        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Category>> FindByIdAsync(CategoryIdentifier id,
        CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Categories
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            return SearchEntityResult<Category>.NotFound();

        var entity = dbEntity.MapToDomainEntity();
        return SearchEntityResult<Category>.Found(entity);
    }

    private async Task WriteEventAsync(Category entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.Events.AddAsync(dbEventEntity, cancellationToken);
    }
}