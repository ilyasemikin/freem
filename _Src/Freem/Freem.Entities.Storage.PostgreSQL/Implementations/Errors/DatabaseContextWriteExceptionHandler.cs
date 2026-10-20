using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Storage.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Abstractions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors;

internal sealed class DatabaseContextWriteExceptionHandler
{
    private readonly DatabaseForeignKeyConstraintErrorToErrorConverter _foreignKeyConstraintConverter;
    private readonly TriggerConstraintErrorToErrorConverter _triggerConstraintConverter;
    private readonly DuplicateKeyErrorToErrorConverter _duplicateKeyConverter;

    public DatabaseContextWriteExceptionHandler(
        DatabaseForeignKeyConstraintErrorToErrorConverter foreignKeyConstraintConverter,
        TriggerConstraintErrorToErrorConverter triggerConstraintConverter,
        DuplicateKeyErrorToErrorConverter duplicateKeyConverter)
    {
        ArgumentNullException.ThrowIfNull(foreignKeyConstraintConverter);
        ArgumentNullException.ThrowIfNull(triggerConstraintConverter);
        ArgumentNullException.ThrowIfNull(duplicateKeyConverter);

        _foreignKeyConstraintConverter = foreignKeyConstraintConverter;
        _triggerConstraintConverter = triggerConstraintConverter;
        _duplicateKeyConverter = duplicateKeyConverter;
    }

    public async Task<Result<Error>> Handle(DatabaseContextWriteContext context, Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (DbUpdateException dbUpdateException) when (dbUpdateException.InnerException is PostgresException e)
        {
            return ConvertException(context, e);
        }
        catch (PostgresException e)
        {
            return ConvertException(context, e);
        }

        return Result<Error>.CreateSuccess();
    }

    private Error ConvertException(DatabaseContextWriteContext context, PostgresException exception)
    {
        if (!IDatabaseError.TryParse(exception, out var databaseError) || 
            !TryConvert(context, databaseError, out var error))
            throw new InternalStorageException(exception.MessageText, exception);
        
        return error;
    }

    private bool TryConvert(
        DatabaseContextWriteContext context,
        IDatabaseError databaseError,
        [NotNullWhen(true)] out Error? error)
    {
        switch (databaseError)
        {
            case DatabaseForeignKeyConstraintError foreignKeyConstraintError:
                return _foreignKeyConstraintConverter.TryConvert(foreignKeyConstraintError, out error);
            case TriggerConstraintError triggerConstraintError:
                error = _triggerConstraintConverter.Convert(triggerConstraintError);
                return true;
            case DuplicateKeyError duplicateKeyError:
                return _duplicateKeyConverter.TryConvert(context, duplicateKeyError, out error);
            default:
                error = null;
                return false;
        }
    }
}