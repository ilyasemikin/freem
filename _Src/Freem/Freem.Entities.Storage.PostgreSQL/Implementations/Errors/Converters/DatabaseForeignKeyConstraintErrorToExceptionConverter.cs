using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Storage.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Converters;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DatabaseForeignKeyConstraintErrorToExceptionConverter
{
    private readonly DatabaseColumnToIdPossibleConverter _columnToIdConverter;

    public DatabaseForeignKeyConstraintErrorToExceptionConverter(
        DatabaseColumnToIdPossibleConverter columnToIdConverter)
    {
        _columnToIdConverter = columnToIdConverter;
    }

    public bool TryConvert(DatabaseContextWriteContext context, DatabaseForeignKeyConstraintError error, [NotNullWhen(true)] out Exception? output)
    {
        output = null;
        if (!_columnToIdConverter.TryConvert(error.Column, out var id))
            return false;

        output = new NotFoundRelatedException(context.ProcessedId, id);
        return true;
    }
}