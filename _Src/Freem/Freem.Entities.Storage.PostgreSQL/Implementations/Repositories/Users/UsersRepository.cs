using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Users;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly DatabaseContext _context;

    public UsersRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();

        await _context.Users.AddAsync(dbEntity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = await _context.Users.FirstOrDefaultAsync(
            e => e.Id == entity.Id.Value,
            cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(UserIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        await _context.Users
            .Where(e => e.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<User>> FindByIdAsync(
        UserIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _context.Users.FindAsync(e => e.Id == id.Value, UserMapper.MapToDomainEntity, cancellationToken);
    }
}
