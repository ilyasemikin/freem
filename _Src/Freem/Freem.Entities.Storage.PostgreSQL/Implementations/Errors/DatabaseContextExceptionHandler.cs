using Freem.Converters.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextExceptionHandler
{
    private readonly ConvertersCollection<IDatabaseError, Exception> _converters;

    public DatabaseContextExceptionHandler(ConvertersCollection<IDatabaseError, Exception> converters)
    {
        _converters = converters;
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
        if (!IDatabaseError.TryParse(exception.Message, out var error) || 
            !_converters.TryConvert(error, out var dbException))
            throw exception;

        throw dbException;
    }
}