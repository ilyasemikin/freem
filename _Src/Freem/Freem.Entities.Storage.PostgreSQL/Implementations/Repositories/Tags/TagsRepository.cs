using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Factories;
using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal sealed class TagsRepository : ITagsRepository
{
    private readonly DatabaseContext _context;

    private readonly IEventEntityFactory<TagEvent, EventIdentifier, UserIdentifier, Tag, TagIdentifier> _eventFactory;

    public TagsRepository(
        DatabaseContext context,
        IEventEntityFactory<TagEvent, EventIdentifier, UserIdentifier, Tag, TagIdentifier> eventFactory)
    {
        _context = context;
        _eventFactory = eventFactory;
    }

    public async Task CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();

        await _context.Tags.AddAsync(dbEntity, cancellationToken);

        await WriteEventAsync(entity, EventAction.Created, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Tags.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value && e.UserId == entity.Id.Value, 
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        dbEntity.Name = entity.Name;

        await WriteEventAsync(entity, EventAction.Updated, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _context.Tags.FirstOrDefaultAsync(e => e.Id == id.Value, cancellationToken);

        if (dbEntity is null)
            throw new NotFoundException(id);

        var entity = dbEntity.MapToDomainEntity();
        await WriteEventAsync(entity, EventAction.Removed, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Tag>> FindByIdAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        return await _context.Tags
            .AsNoTracking()
            .FindAsync(e => e.Id == id.Value, TagMapper.MapToDomainEntity);
    }

    private async Task WriteEventAsync(Tag entity, EventAction action, CancellationToken cancellationToken)
    {
        var eventEntity = _eventFactory.Create(entity, action);
        var dbEventEntity = eventEntity.MapToDatabaseEntity();
        await _context.AddAsync(dbEventEntity, cancellationToken);
    }
}
