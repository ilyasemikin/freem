using Freem.Entities.Storage.PostgreSQL.Database.Errors;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Factories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class ContextExceptionHandler
{
    private readonly IStorageExceptionFactory _factory;

    public ContextExceptionHandler(IStorageExceptionFactory factory)
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
        if (!Error.TryParse(exception.Message, out var error))
            throw exception;
        
        throw _factory.Create(error);
    }
}