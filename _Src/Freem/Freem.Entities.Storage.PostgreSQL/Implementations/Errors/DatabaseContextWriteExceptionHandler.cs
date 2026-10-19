using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteExceptionHandler
{
    private readonly DatabaseForeignKeyConstraintErrorToExceptionConverter _foreignKeyConstraintConverter;
    private readonly TriggerConstraintErrorToExceptionConverter _triggerConstraintConverter;
    private readonly DuplicateKeyErrorToExceptionConverter _duplicateKeyConverter;

    public DatabaseContextWriteExceptionHandler(
        DatabaseForeignKeyConstraintErrorToExceptionConverter foreignKeyConstraintConverter,
        TriggerConstraintErrorToExceptionConverter triggerConstraintConverter,
        DuplicateKeyErrorToExceptionConverter duplicateKeyConverter)
    {
        ArgumentNullException.ThrowIfNull(foreignKeyConstraintConverter);
        ArgumentNullException.ThrowIfNull(triggerConstraintConverter);
        ArgumentNullException.ThrowIfNull(duplicateKeyConverter);

        _foreignKeyConstraintConverter = foreignKeyConstraintConverter;
        _triggerConstraintConverter = triggerConstraintConverter;
        _duplicateKeyConverter = duplicateKeyConverter;
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
            !TryConvert(context, error, out var dbException))
            throw new InternalStorageException(exception.MessageText, exception);
        
        throw dbException;
    }

    private bool TryConvert(
        DatabaseContextWriteContext context,
        IDatabaseError error,
        [NotNullWhen(true)] out Exception? exception)
    {
        switch (error)
        {
            case DatabaseForeignKeyConstraintError foreignKeyConstraintError:
                return _foreignKeyConstraintConverter.TryConvert(context, foreignKeyConstraintError, out exception);
            case TriggerConstraintError triggerConstraintError:
                exception = _triggerConstraintConverter.Convert(context, triggerConstraintError);
                return true;
            case DuplicateKeyError duplicateKeyError:
                return _duplicateKeyConverter.TryConvert(context, duplicateKeyError, out exception);
            default:
                exception = null;
                return false;
        }
    }
}