using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Converters;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DatabaseForeignKeyConstraintErrorToExceptionConverter
{
    private readonly DatabaseColumnToIdentifierPossibleConverter _columnToIdentifierConverter;

    public DatabaseForeignKeyConstraintErrorToExceptionConverter(
        DatabaseColumnToIdentifierPossibleConverter columnToIdentifierConverter)
    {
        _columnToIdentifierConverter = columnToIdentifierConverter;
    }

    public bool TryConvert(DatabaseContextWriteContext context, DatabaseForeignKeyConstraintError error, [NotNullWhen(true)] out Exception? output)
    {
        output = null;
        if (!_columnToIdentifierConverter.TryConvert(error.Column, out var identifier))
            return false;

        output = new NotFoundRelatedException(context.ProcessedId, identifier);
        return true;
    }
}