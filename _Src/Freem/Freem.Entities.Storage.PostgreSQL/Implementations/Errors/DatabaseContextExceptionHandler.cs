using Freem.Entities.Storage.PostgreSQL.Database.Errors;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextExceptionHandler
{
    private readonly IDatabaseStorageExceptionFactory _factory;

    public DatabaseContextExceptionHandler(IDatabaseStorageExceptionFactory factory)
    {
        _factory = factory;
    }

    public async Task Handle(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException is PostgresException e)
        {
            ThrowException(e);
        }
        catch (PostgresException e)
        {
            ThrowException(e);
        }
    }

    private void ThrowException(PostgresException exception)
    {
        if (!IDatabaseError.TryParse(exception.Message, out var error))
            throw exception;
        
        throw _factory.Create(error);
    }
}