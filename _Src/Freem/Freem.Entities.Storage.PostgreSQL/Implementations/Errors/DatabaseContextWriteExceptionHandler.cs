using Freem.Converters.Collections;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteExceptionHandler
{
    private readonly ConvertersCollection<DatabaseContextWriteContext, IDatabaseError, Exception> _converters;

    public DatabaseContextWriteExceptionHandler(
        ConvertersCollection<DatabaseContextWriteContext, IDatabaseError, Exception> converters)
    {
        _converters = converters;
    }

    public async Task Handle(DatabaseContextWriteContext context, Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException is PostgresException e)
        {
            ThrowException(context, e);
        }
        catch (PostgresException e)
        {
            ThrowException(context, e);
        }
    }

    private void ThrowException(DatabaseContextWriteContext context, PostgresException exception)
    {
        if (!IDatabaseError.TryParse(exception, out var error) || 
            !_converters.TryConvert(context, error, out var dbException))
            throw new InternalStorageException(exception.MessageText, exception);
        
        throw dbException;
    }
}