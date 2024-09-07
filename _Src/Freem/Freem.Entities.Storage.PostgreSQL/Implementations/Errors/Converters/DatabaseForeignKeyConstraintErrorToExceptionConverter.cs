using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Models;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DatabaseForeignKeyConstraintErrorToExceptionConverter
    : IPossibleConverter<DatabaseForeignKeyConstraintError, Exception>
{
    private readonly IPossibleConverter<DatabaseColumnWithValue, IEntityIdentifier> _columnToIdentifierConverter;

    public DatabaseForeignKeyConstraintErrorToExceptionConverter(
        IPossibleConverter<DatabaseColumnWithValue, IEntityIdentifier> columnToIdentifierConverter)
    {
        _columnToIdentifierConverter = columnToIdentifierConverter;
    }

    public bool TryConvert(DatabaseForeignKeyConstraintError input, out Exception output)
    {
        output = null!;
        if (!_columnToIdentifierConverter.TryConvert(input.Column, out var identifier))
            return false;

        output = new NotFoundRelatedException(identifier);
        return true;
    }
}