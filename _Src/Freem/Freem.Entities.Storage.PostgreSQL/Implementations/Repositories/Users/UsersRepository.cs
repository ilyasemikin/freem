using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Users;

internal sealed class UsersRepository : IUsersRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;
    private readonly IEqualityComparer<User> _equalityComparer;

    public UsersRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler,
        IEqualityComparer<User> equalityComparer)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
        _equalityComparer = equalityComparer;
    }

    public async Task CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();

        await _database.Users.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Users.FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            throw new NotFoundException(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (_equalityComparer.Equals(entity, actualEntity))
            return;

        var context = new DatabaseContextWriteContext(entity.Id);
        await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task DeleteAsync(UserIdentifier id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        await _database.Users
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<SearchEntityResult<User>> FindByIdAsync(
        UserIdentifier id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        return await _database.Users.FindAsync(e => e.Id == id, UserMapper.MapToDomainEntity, cancellationToken);
    }
}
