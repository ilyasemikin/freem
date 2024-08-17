using Freem.Entities.Abstractions.Identifiers.Extensions;
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

    public TagsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        var dbEntity = entity.MapToDatabaseEntity();

        await _context.Tags.AddAsync(dbEntity, cancellationToken);

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

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Tags
            .Where(e => e.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);

        if (count == 0)
            throw new NotFoundException(id);
    }

    public async Task<int> RemoveMultipleByUserAsync(UserIdentifier userId, IEnumerable<TagIdentifier> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Tags
            .Where(e => e.UserId == userId.Value && ids.AsValues().Contains(e.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<Tag>> FindByIdAsync(TagIdentifier id, CancellationToken cancellationToken = default)
    {
        return await _context.Tags
            .AsNoTracking()
            .FindAsync(e => e.Id == id.Value, TagMapper.MapToDomainEntity);
    }
}
