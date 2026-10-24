using Freem.Domain.Entities.Users;
using Freem.Domain.Entities.Users.Comparers;
using Freem.Domain.Storage.Repositories.Users;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors;
using Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Extensions;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Users;

internal sealed class UsersRepository : IUserRepository
{
    private readonly DatabaseContext _database;
    private readonly DatabaseContextWriteExceptionHandler _exceptionHandler;

    public UsersRepository(
        DatabaseContext database,
        DatabaseContextWriteExceptionHandler exceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(exceptionHandler);
        
        _database = database;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<Result<Error>> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var dbEntity = entity.MapToDatabaseEntity();

        await _database.Users.AddAsync(dbEntity, cancellationToken);

        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var dbEntity = await _database.Users
            .Include(e => e.Settings)
            .FindEntityAsync(entity, cancellationToken);
        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateUserNotFound(entity.Id);

        var actualEntity = dbEntity.MapToDomainEntity();
        if (UserEqualityComparer.Instance.Equals(entity, actualEntity))
            return Result<Error>.CreateSuccess();

        dbEntity.Settings = entity.Settings.MapToDatabaseEntity(entity.Id);
        
        var context = new DatabaseContextWriteContext(entity.Id);
        return await _exceptionHandler.HandleSaveChangesAsync(context, _database, cancellationToken);
    }

    public async Task<Result<Error>> DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        var idString = id.ToString();
        await _database.Users
            .Where(e => e.Id == idString)
            .ExecuteDeleteAsync(cancellationToken);

        return Result<Error>.CreateSuccess();
    }

    public async Task<Result<User, Error>> FindByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var idString = id.ToString();
        return await _database.Users
            .Include(e => e.Settings)
            .FindAsync(
                () => DomainErrorCatalog.Factories.CreateUserNotFound(id),
                e => e.Id == idString,
                UserMapper.MapToDomainEntity,
                cancellationToken);
    }
}
